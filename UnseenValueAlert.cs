using Seq.Apps;
using Seq.Apps.LogEvents;

namespace Seq.App.UnseenValueAlert
{
    [SeqApp("UnseenValue Alert",
      Description = "Alerts you if a property value does not match the predefined whitelist")]
    public class UnseenValueAlert : Reactor, ISubscribeTo<LogEventData>
    {
        [SeqAppSetting(
           DisplayName = "Known Values",
           HelpText = "The values that you already know about. Comma delimted (,)",
           InputType = SettingInputType.LongText)]
        public string Settings_KnownValues { get; set; }

        [SeqAppSetting(
         DisplayName = "Event Property To Watch",
         HelpText = "Which event property do you want to be alerted for",
         InputType = SettingInputType.Text)]
        public string Settings_PropertyToWatch { get; set; }

        [SeqAppSetting(
         DisplayName = "Alert template",
         HelpText = "What message must be shown for the new value? ",
         InputType = SettingInputType.Text)]
        public string Settings_AlertTemplate { get; set; }

        public void On(Event<LogEventData> evt)
        {
            //get the header property of the event
            string propertyValue = string.Empty;
            var eventPropertyValue = evt.Data.Properties[Settings_PropertyToWatch];
            if (eventPropertyValue != null)
            {
                propertyValue = eventPropertyValue.ToString();
            }

            if (!string.IsNullOrWhiteSpace(propertyValue) && !IsKnownCall(propertyValue))
            {
                Log.Information(Settings_AlertTemplate + " - {NewValue}", propertyValue);
            }
        }

        private bool IsKnownCall(string valueToCheck)
        {
            valueToCheck = valueToCheck.ToUpper();

            var isKnownCall = false;
            isKnownCall = Settings_KnownValues.ToUpper().Contains(valueToCheck);   
            return isKnownCall;
        }
    }
}
