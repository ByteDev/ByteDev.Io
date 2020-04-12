namespace ByteDev.Io.IsolatedStorage
{
    /// <summary>
    /// Isolated storage file store type.
    /// </summary>
    public enum IsolatedStorageFileType
    {
        /// <summary>
        /// Machine scoped isolated storage corresponding to the calling code's application identity.
        /// </summary>
        MachineStoreForApplication = 1,

        /// <summary>
        /// Machine scoped isolated storage corresponding to the calling code's assembly identity.
        /// </summary>
        MachineStoreForAssembly = 2,

        /// <summary>
        /// Machine scoped isolated storage corresponding to the application domain identity and the assembly identity.
        /// </summary>
        MachineStoreForDomain = 3,

        /// <summary>
        /// User scoped isolated storage corresponding to the calling code's application identity.
        /// </summary>
        UserStoreForApplication = 4,

        /// <summary>
        /// User scoped isolated storage corresponding to the calling code's assembly identity.
        /// </summary>
        UserStoreForAssembly = 5,

        /// <summary>
        /// User scoped isolated storage corresponding to the application domain identity and the assembly identity.
        /// </summary>
        UserStoreForDomain = 6
    }
}