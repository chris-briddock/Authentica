namespace Api.Constants;

public static partial class Routes
{
    /// <inheritdoc/>
    public static partial class Users
    {
        /// <summary>
        /// Routes for passkey authentication.
        /// </summary>
        public static class Passkeys
        {
            /// <summary>
            /// Route for passkey attestation options.
            /// </summary>
            public const string PasskeyAttestationOptions = "users/passkeys/attestation/options";
            /// <summary>
            /// Route for passkey attestation. 
            /// </summary>
            public const string PasskeyAttestation = "users/passkeys/attestation";
            /// <summary>
            /// Route for passkey assertion.
            /// </summary>
            public const string PasskeyAssertion = "users/passkeys/assertion";
            /// <summary>
            /// Route for passkey assertion options.
            /// </summary>
            public const string PasskeysAssertionOptions = "users/passkeys/assertion/options";
        }
        
    }
}