using Microsoft.AspNetCore.Builder;

namespace backend_recaptcha.Middlewares
{
	public static class CustomStaticMiddleware
	{
		public static IApplicationBuilder UseGoogleRecaptchaValidation(this IApplicationBuilder builder)
		{
			return builder.UseMiddleware<RecaptchaMiddleware>();
		}
	}
}