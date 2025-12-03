// ╔═════════════════════════════════════════════════════════════════════════════╗
// ║                                                                             ║
// ║   File:        TestCollection.cs                                            ║
// ║   Created:     December 3, 2025                                             ║
// ║   Description: Defines the serial test collection for xUnit tests           ║
// ║                                                                             ║
// ╚═════════════════════════════════════════════════════════════════════════════╝

using Xunit;

namespace EasyLogger.Tests;

/// <summary>
/// Defines an xUnit test collection for serial test execution by disabling parallelization.
/// </summary>
[CollectionDefinition("SerialCollection", DisableParallelization = true)]
public class SerialCollection {
    // This class has no code, and is never created. Its purpose is simply
    // to be the place to apply [CollectionDefinition] and all the
    // ICollectionFixture<> interfaces.
}
