using System;

namespace WebLib
{
    public class AppConstant
    {
        public const String APP_NAME = "AppName";
        public const String UOBI_KEY = "UOBIKey";
        public const String DOMAIN = "Domain";
        public const String AD_PATH = "ADPath";
    }

    public class SortConstant
    {
        public const String ASC = "ASC";
        public const String DESC = "DESC";
    }

    public class ErrorConstant
    {
        public const String SP_NAME = "EXEC USP_INSERT_ADM_ERROR_LOG";
        public const String ERR_NOT_AUTHORIZED = "You are not authorized to access this page.";
        public const String ERR_CONTACT_ADMIN = "You cannot login. Please contact your administrator.";
    }

    public class ObjectRepoConstant
    {
        public const String TABLE_NAME ="TableName";
        public const String EXT_PROC = "USP_";
        public const String PROC_STATUS = "Status";
    }

    public class SPName
    {
        public const String SP_PAGING = "USP_getPaging";
    }

    public class SPStatus
    {
        public const String SP_INSERT = "Insert";
        public const String SP_UPDATE = "Update";
        public const String SP_DELETE = "Delete";
    }

    public class ConnectorConstant
    {
        public const String CON_AND = " AND ";
        public const String CON_OR = " OR ";
    }

    public class OperatorConstant
    {
        public const String OP_EQUAL = " = ";
        public const String OP_NOT_EQUAL = " != ";
        public const String OP_MORE_THEN = " > ";
        public const String OP_MORE_THEN_EQUAL = " >= ";
        public const String OP_LESS_THEN = " < ";
        public const String OP_LESS_THEN_EQUAL = " <= ";
        public const String OP_LIKE = " like ";
        public const String OP_BETWEEN = " BETWEEN ";
        public const String OP_IN = " IN ";
    }
}
