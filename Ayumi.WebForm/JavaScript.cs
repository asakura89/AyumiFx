namespace WebLib
{
    public class JavaObject
    {
        public string Value = "";
        public string Type = "";
        public string Parameter = "";
    }
    /// <summary>
    /// class to validate with javascript
    /// </summary>
    public class JavaScript
    {
        public JavaObject ScriptAlert; 
        public JavaObject ScriptWindowOpen;
        public JavaObject ScriptWindowStatus;
        public JavaObject EventConfirm;
        public JavaObject EventCurrency;
        public JavaObject EventCurrencyRange;
        public JavaObject EventBack;
        public JavaObject EventNumeric;
        public JavaObject EventFormatNumericRange;
        public JavaObject EventUppercase;
        public JavaObject EventLowercase;
        public JavaObject EventEmailAddress;
        public JavaObject FuncEmailValidation;
        public JavaObject FuncNumericValidation;
        public JavaObject FuncFormatNumericRange;
        public JavaObject FuncFormatCurrency;
        public JavaObject FuncFormatCurrencyRange;

        public JavaScript()
        {
            // Javascript for format currency
            /*FuncFormatCurrency.Value = "function formatCurrency(num) {num = num.toString().replace(/\\$|\\,/g,''); if(isNaN(num)) num = '0';	sign = (num == (num = Math.abs(num)));	num = Math.floor(num*100+0.50000000001); cents = num%100; num = Math.floor(num/100).toString();	if(cents<10) cents = '0' + cents; return (((sign)?'':'-') + num + '.' + cents);}";
            FuncFormatCurrency.Type = "SCRIPT";

            // Javascript for format currencyRange
            FuncFormatCurrencyRange.Value = "function formatCurrencyRange(num,min,max) {num = num.toString().replace(/\\$|\\,/g,''); if(isNaN(num) || (num < min) || (num > max)) num = min;	sign = (num == (num = Math.abs(num)));	num = Math.floor(num*100+0.50000000001); cents = num%100; num = Math.floor(num/100).toString();	if(cents<10) cents = '0' + cents; return (((sign)?'':'-') + num + '.' + cents);}";
            FuncFormatCurrencyRange.Type = "SCRIPT";

            // Javascript for numeric validation
            FuncNumericValidation.Value = "function formatNumeric(num) {num = num.toString().replace(/\\$|\\,/g,''); if(isNaN(num)) num = '0'; return num;}";
            FuncNumericValidation.Type = "SCRIPT";

            // Javascript for numeric range
            FuncFormatNumericRange.Value = "function formatNumericRange(num,min,max) {num = num.toString().replace(/\\$|\\,/g,''); if(isNaN(num) || (num < min) || (num > max)) num = min; return num;}";
            FuncFormatNumericRange.Type = "SCRIPT";

            // Javascript for email validation
            FuncEmailValidation.Value = "function validate(form_id,email) {"
               +"var reg = /^([A-Za-z0-9_\\-\\.])+\\@([A-Za-z0-9_\\-\\.])+\\.([A-Za-z]{2,4})$/;"
               +"var address = document.forms[form_id].elements[email].value;"
               +"if(reg.test(address) == false) { alert('Invalid Email Address'); return false;}}";
            FuncEmailValidation.Type = "SCRIPT";

            // Javascript for give an alert message
            ScriptAlert.Value = "alert('@Message');";
            ScriptAlert.Type = "<SCRIPT>";
            ScriptAlert.Parameter = "@Message"; 

            //Javascript for open window
            ScriptWindowOpen.Value = "window.open('@Location','@Target');";
            ScriptWindowOpen.Type = "<SCRIPT>";
            ScriptWindowOpen.Parameter = "@Location,@Target";

            // Javascript for windows status
            ScriptWindowStatus.Value = "window.status = '@Value';";
            ScriptWindowStatus.Type = "<SCRIPT>";
            ScriptWindowStatus.Parameter = "@Value";

            // Javascript for event confirm
            EventConfirm.Value = "javascript:{return window.confirm('@Message');}";
            EventConfirm.Type = "EVENT";
            EventConfirm.Parameter = "@Message";

            // Javascript for event currency
            EventCurrency.Value = "javascript:{this.value=formatCurrency(this.value);}";
            EventCurrency.Type = "EVENT";

            // Javascript for currency range
            EventCurrencyRange.Value = "javascript:{this.value=formatCurrencyRange(this.value,@min,@max);}";
            EventCurrencyRange.Type = "EVENT";

            // Javascript for back to page before
            EventBack.Value = "javascript:{history.back(1);}";
            EventBack.Type = "EVENT";

            // Javascript for numeric value
            EventNumeric.Value = "javascript:{this.value=formatNumeric(this.value);}";
            EventNumeric.Type = "EVENT";

            // Javascript for numeric range
            EventFormatNumericRange.Value = "javascript:{this.value=formatNumericRange(this.value,@min,@max);}";
            EventFormatNumericRange.Type = "EVENT";
            EventFormatNumericRange.Parameter = "@max,@min";

            // Javascript for uppercase
            EventUppercase.Value = "javascript:{this.value = this.value.toUpperCase();}";
            EventUppercase.Type = "EVENT";

            // Javascript for lowercase
            EventLowercase.Value = "javascript:{this.value = this.value.toLowerCase();}";
            EventLowercase.Type = "EVENT";

            // Javascript for validemail
            EventEmailAddress.Value = "javascript:{this.value = ValidEmail(this.value);}";
            EventEmailAddress.Type = "EVENT";*/          

        }
    }
}
