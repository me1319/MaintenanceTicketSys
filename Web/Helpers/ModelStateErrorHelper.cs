namespace Web.Helpers
{
    public class ModelStateErrorHelper
    {
        public static string NormalizeModelError(string error)
        {
            if (error.Contains("could not be converted"))
                return "Invalid value provided";

            if (error.Contains("field is required"))
                return "Required field is missing";

            return error;
        }
    }
}
