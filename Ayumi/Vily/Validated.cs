using System;
using System.Collections.Generic;
using System.Linq;
using Arvy;

namespace Vily {
    public class Validated<TData> {
        public IList<ActionResponseViewModel> Messages { get; set; } = new List<ActionResponseViewModel>();
        public IList<TData> ValidData { get; set; } = new List<TData>();
        public Boolean ContainsFail => Messages.Any(message => message.ResponseType == ActionResponseViewModel.Error);
        public Boolean AllFails => Messages.All(message => message.ResponseType == ActionResponseViewModel.Error);
    }
}
