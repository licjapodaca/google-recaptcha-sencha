
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace backend_recaptcha.Middlewares
{
	public class RecaptchaMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly IConfiguration _config;

		private HttpClient _httpClient;

		public RecaptchaMiddleware(RequestDelegate next, IConfiguration config, IHttpClientFactory clientFactory)
		{
			_config = config;
			_next = next;
			_httpClient = clientFactory.CreateClient();
		}

		public async Task InvokeAsync(HttpContext context)
		{
			// Logic to perform on request
			if (context.Request.Method.ToUpper() == "POST")
			{
				if (string.IsNullOrEmpty(context.Request.Headers["GoogleRecaptchaToken"]))
				{
					context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
					context.Response.ContentType = "application/json";
					await context.Response.WriteAsync($"{{ \"success\": false, \"message\": \"Falta que se incluya el Header GoogleRecaptchaToken\" }}");
				}
				else
				{
					var result = await ValidarGoogleTokenAsync(
						GoogleRecaptchaToken: context.Request.Headers["GoogleRecaptchaToken"]
						);
					// Validacion de Bot con reCaptcha de Google
					if (result.Item1)
					{
						await _next(context);
					}
					else
					{
						context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
						context.Response.ContentType = "application/json";
						await context.Response.WriteAsync($"{{ \"success\": false, \"message\": \"Esto es un Bot {result.Item2}\" }}");
					}
				}
			}
			else
			{
				await _next(context);
			}
		}

		private async Task<Tuple<bool,string>> ValidarGoogleTokenAsync(
			string GoogleRecaptchaToken
		)
		{
			var request = new HttpRequestMessage(HttpMethod.Post,
				$"{_config.GetValue<string>("GoogleRecaptchaUrlValidationToken")}?secret={_config.GetValue<string>("GoogleRecaptchaSecretKey")}&response={GoogleRecaptchaToken}"
				);

			//request.Headers.Add("x-functions-key", functionKey); //.Authorization = new AuthenticationHeaderValue("Bearer", result.AccessToken);

			// if (!string.IsNullOrEmpty(jsonData))
			// {
			// 	request.Content = new StringContent(jsonData, Encoding.UTF8, "application/json");
			// }

			HttpResponseMessage response = await _httpClient.SendAsync(request);

			if (!response.IsSuccessStatusCode)
			{
				string error = await response.Content.ReadAsStringAsync();
				object formatted = JsonConvert.DeserializeObject(error);
				throw new WebException($"Error: \n{JsonConvert.SerializeObject(formatted, Formatting.Indented)}");
			}
			else
			{
				string json = await response.Content.ReadAsStringAsync();
				var result = JsonConvert.DeserializeObject<RecaptchaModel>(json);
				
				if(result.score > 0.5)
				{
					return Tuple.Create(true, "");
				}
				else
				{
					return Tuple.Create(false, result.errors.FirstOrDefault());
				}
			}
		}
	}

	public class RecaptchaModel
	{
		/*
		{
		"success": true|false,      // whether this request was a valid reCAPTCHA token for your site
		"score": number             // the score for this request (0.0 - 1.0)
		"action": string            // the action name for this request (important to verify)
		"challenge_ts": timestamp,  // timestamp of the challenge load (ISO format yyyy-MM-dd'T'HH:mm:ssZZ)
		"hostname": string,         // the hostname of the site where the reCAPTCHA was solved
		"error-codes": [...]        // optional
		}
		*/
		public bool success { get; set; }
		public double score { get; set; }
		public string action { get; set; }
		public string hostname { get; set; }
		[JsonProperty(PropertyName = "error-codes")]
		public List<string> errors { get; set; }
	}
}