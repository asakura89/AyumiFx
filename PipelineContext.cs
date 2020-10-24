using System;
using System.Collections.Generic;
using Arvy;

namespace Ria {
    public class PipelineContext {
        public IList<ActionResponseViewModel> ActionMessages { get; set; } = new List<ActionResponseViewModel>();
        public Object Context { get; set; }
        public Boolean Cancelled { get; set; }
    }
}
