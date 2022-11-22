namespace VesselWebCenter.Data.Constants
{
    public static class GlobalConstants
    {
        public const string PASSWORD_VALIDATE_EXPRESSION = "^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z])(?=.*\\W)(?!.* ).{6,24}$";
        public const string PASSWORD_REQUIREMENTS = "Password must be between 6 and 24 characters long. It must contain at least one digit, upper-case letter, lower-case letter and one special character!";

        public const string CHOOSE_ACCOUNT_TO_DELETE = "Please, choose one of the listed accounts which are to be deleted";
        public const string CHOOSE_ACCOUNT_TO_RECOVER = "Please, choose a valid account you want to recover";
        public const string NAME_FIELD_REQUIREMENTS = "This field requires Upper-case starting character, followed by not more than 20 Lower-case characters only";
        public const string FIRST_NAME_REQUIREMENTS = "This field requires Upper-case starting character, followed by not more than 20 Lower-case characters only";

        public const string TEXT_FIELD_REQUIREMENTS = "Field must be a text";
        
    }    
    public static class RoleConstants 
    {
        public const string ADMINISTRATOR = "Administrator";
        public const string MANAGER = "Manager";
        public const string USER_OWNER = "User-Owner";
        public const string REGULAR_USER = "Ordinary-User";
        

        public const string SUCCESSFUL_ROLE_ADD = "The role was given successfully to the appropriate user.";
        public const string ROLE_ALREADY_EXISTS = "This account already have such role added !";
        public const string SELECT_RELEVANT_ROLE_AND_ACCOUNT = "Please, select the appropriate account and relevant role for it!";
        public const string RESET_ROLES = "All roles have been deleted from current user account";

    }
}
