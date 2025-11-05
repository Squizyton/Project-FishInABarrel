namespace Service_Locator
{
    public interface IService
    {
        /// <summary>
        /// Called when the service is added to the service locator
        /// </summary>
        void ServiceAdded();
        /// <summary>
        /// Called when the service is removed from the service locator
        /// </summary>
        void RemoveService();
        /// <summary>
        /// Called when the service is located.
        /// </summary>
        void OnLocate();
    }
}