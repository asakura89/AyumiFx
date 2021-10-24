using Arvy;

namespace WebApp {
    public interface IUnitOfWork {
        ActionResponseViewModel Submit();
    }
}