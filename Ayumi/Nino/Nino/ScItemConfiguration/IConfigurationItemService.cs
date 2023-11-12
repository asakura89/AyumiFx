using System;

namespace Nino.ScItemConfiguration {
    public interface IConfigurationItemService {
        String GetValue();
        void SetValue(String value);
        String GetDescription();
        void SetDescription(String desc);
    }
}