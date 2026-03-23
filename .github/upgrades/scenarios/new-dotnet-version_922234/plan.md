# .NET 10.0 Upgrade Plan for robert-brands-com

## Table of Contents

- [Executive Summary](#executive-summary)
- [Migration Strategy](#migration-strategy)
- [Detailed Dependency Analysis](#detailed-dependency-analysis)
- [Project-by-Project Plans](#project-by-project-plans)
  - [robert-brands-com.csproj](#robert-brands-comcsproj)
- [Package Update Reference](#package-update-reference)
- [Breaking Changes Catalog](#breaking-changes-catalog)
- [Risk Management](#risk-management)
- [Testing & Validation Strategy](#testing--validation-strategy)
- [Complexity & Effort Assessment](#complexity--effort-assessment)
- [Source Control Strategy](#source-control-strategy)
- [Success Criteria](#success-criteria)

---

## Executive Summary

### Scenario Overview

This plan details the upgrade of the robert-brands-com ASP.NET Core Razor Pages application from **.NET 8.0** to **.NET 10.0 (Long Term Support)**.

### Scope

- **Projects Affected**: 1 project (robert-brands-com.csproj)
- **Current State**: net8.0, 11 NuGet packages, 8,151 lines of code
- **Target State**: net10.0, updated packages, resolved API compatibility issues
- **Codebase Type**: ASP.NET Core Razor Pages web application with Azure integration

### Discovered Metrics

| Metric | Value | Assessment |
| :--- | :--- | :--- |
| Total Projects | 1 | Simple - single project |
| Dependency Depth | 0 | No project dependencies |
| Lines of Code | 8,151 | Low complexity |
| Code Files | 105 (4 with incidents) | Focused impact |
| NuGet Packages | 11 (1 upgrade, 1 replacement) | Minimal package changes |
| API Issues | 12 (11 binary incompatible, 1 behavioral) | Addressable breaking changes |
| Estimated LOC to Modify | 12+ (~0.1%) | Very low impact |

### Complexity Classification

**Simple Solution** - Single project, no dependencies, low complexity, clear upgrade path

**Rationale**:
- ✅ Single ASP.NET Core project with no project dependencies
- ✅ Small to medium codebase (8.1K LOC)
- ✅ Only 4 code files affected by API changes
- ✅ All packages have clear upgrade/replacement paths
- ✅ No security vulnerabilities identified
- ✅ SDK-style project (modern format)

### Selected Strategy

**All-At-Once Strategy** - Upgrade all components simultaneously in a single coordinated operation.

**Rationale**:
- Single project makes phased approach unnecessary
- No dependency coordination required
- Fast completion time (estimated single session)
- Low risk due to small surface area
- Clean, atomic upgrade with single testing pass
- Solution from .NET 8 to .NET 10 is well-documented migration path

### Critical Issues

1. **Package Replacement Required**: `Microsoft.AspNetCore.Authentication.AzureAD.UI` (v6.0.36) is obsolete and must be replaced with `Microsoft.Identity.Web` (v4.6.0) patterns
2. **Binary Incompatible APIs**: 11 API calls require code changes (primarily configuration binding methods)
3. **Behavioral Change**: `UseExceptionHandler` extension method has behavior change requiring validation

### Iteration Strategy

**Fast Batch Approach** (2-3 detail iterations):
1. **Foundation** (iterations 2.1-2.3): Strategy, dependency analysis, project stub
2. **Comprehensive Detail** (iteration 3.1): Complete project details, package updates, breaking changes
3. **Finalization** (iteration 3.2): Testing strategy, risk management, success criteria

### Expected Outcome

- Modernized application running on .NET 10.0 LTS
- Updated authentication using Microsoft.Identity.Web patterns
- Resolved API compatibility issues
- Updated package dependencies
- Validated functionality through comprehensive testing

---

## Migration Strategy

### Approach: All-At-Once Strategy

**Selected Approach**: Upgrade all components simultaneously in a single coordinated operation.

### Justification

The All-At-Once strategy is ideal for this upgrade because:

1. **Single Project**: No coordination between multiple projects required
2. **Low Complexity**: 8,151 LOC with only 4 files requiring changes
3. **Minimal Package Changes**: Only 2 packages need attention (1 update, 1 removal)
4. **Clear Upgrade Path**: .NET 8 to .NET 10 migration is well-documented
5. **Fast Completion**: Can be completed in a single session
6. **Lower Risk**: Small surface area reduces chance of issues
7. **Simpler Testing**: Single comprehensive test pass instead of multiple phases

### All-At-Once Strategy Characteristics

- **Atomic Operation**: All changes applied together (framework + packages + code fixes)
- **No Intermediate States**: Project goes directly from .NET 8 to .NET 10
- **Single Testing Phase**: One comprehensive validation after all changes applied
- **Fast Timeline**: Minimal time to complete
- **Clean Coordination**: No multi-targeting or compatibility layers needed

### Execution Approach

**Phase 0: Preparation** (if needed)
- Verify .NET 10 SDK installed
- No global.json in solution (not applicable)

**Phase 1: Atomic Upgrade** (single coordinated operation)
- Update project file TargetFramework property
- Update Microsoft.VisualStudio.Web.CodeGeneration.Design package
- Remove obsolete Microsoft.AspNetCore.Authentication.AzureAD.UI package
- Restore NuGet packages
- Build solution to identify compilation errors
- Fix all binary incompatible API calls
- Address behavioral changes
- Rebuild and verify solution builds with 0 errors

**Phase 2: Validation**
- Run application smoke tests
- Validate authentication flows (Entra ID integration)
- Test Azure service integrations
- Verify all functionality

### Dependency-Based Ordering

Not applicable - single project with no project dependencies. All changes occur atomically.

### Parallel vs Sequential Execution

Not applicable - single project upgrade executed as one atomic operation.

### Risk Mitigation During Migration

- **Version Control**: All changes on dedicated branch `upgrade-to-NET10`
- **Atomic Commits**: Single commit for entire upgrade (or logical grouping)
- **Build Validation**: Ensure solution builds before testing
- **Rollback Plan**: Git branch allows easy rollback if issues arise

---

## Detailed Dependency Analysis

### Dependency Graph Summary

This is a **standalone single-project solution** with no inter-project dependencies. The project has external NuGet package dependencies but no project-to-project references.

```
robert-brands-com.csproj (ASP.NET Core Razor Pages)
├── No project dependencies
└── 11 NuGet package dependencies
```

### Project Grouping

Since there is only one project, no phased migration is required. The entire upgrade occurs as a single atomic operation.

**Single Migration Phase**:
- **robert-brands-com.csproj**: Main ASP.NET Core Razor Pages web application

### Critical Path

The critical path is straightforward:
1. Update project target framework to net10.0
2. Update/replace NuGet packages
3. Resolve API compatibility issues
4. Build and test

### Dependency Considerations

**External Dependencies**:
- **Azure Services**: Azure.Storage.Queues, Microsoft.Azure.Cosmos, Microsoft.ApplicationInsights.AspNetCore
- **Authentication**: Microsoft.Identity.Web (currently also has obsolete Microsoft.AspNetCore.Authentication.AzureAD.UI)
- **HTTP Client**: Flurl, Flurl.Http
- **Utilities**: Ical.Net, reCAPTCHA.AspNetCore, NetEscapades.AspNetCore.SecurityHeaders
- **Tooling**: Microsoft.VisualStudio.Web.CodeGeneration.Design

All packages are compatible with .NET 10.0 except:
- `Microsoft.VisualStudio.Web.CodeGeneration.Design`: Needs version update (8.0.7 → 10.0.2)
- `Microsoft.AspNetCore.Authentication.AzureAD.UI`: Obsolete, needs removal (authentication is already handled by Microsoft.Identity.Web v3.8.3)

### Circular Dependencies

None - single project solution.

### Migration Order

Not applicable - single atomic upgrade.

---

## Project-by-Project Plans

### robert-brands-com.csproj

#### Current State
- **Target Framework**: net8.0
- **Project Type**: ASP.NET Core Razor Pages web application
- **SDK Style**: Yes (modern project format)
- **Dependencies**: 11 NuGet packages
- **Dependants**: None (standalone application)
- **Lines of Code**: 8,151
- **Code Files**: 105 (4 files with API compatibility incidents)

#### Target State
- **Target Framework**: net10.0
- **Updated Packages**: 1 package version update, 1 obsolete package removal
- **API Fixes**: 12 API compatibility issues resolved

#### Migration Steps

##### 1. Prerequisites

**Verify .NET 10 SDK Installation**:
- Ensure .NET 10.0 SDK is installed on development machine
- Verify with: `dotnet --list-sdks`
- Download from: https://dotnet.microsoft.com/download/dotnet/10.0 if needed

**Check for global.json**:
- No global.json file detected in solution
- No SDK version constraints to update

##### 2. Project File Update

**File**: `robert-brands-com.csproj`

Update the TargetFramework property:

```xml
<PropertyGroup>
  <TargetFramework>net10.0</TargetFramework>
  <!-- other properties unchanged -->
</PropertyGroup>
```

**Change**: `net8.0` → `net10.0`

##### 3. Package Reference Updates

Apply all package updates as specified in the [Package Update Reference](#package-update-reference) section:

1. **Update**: `Microsoft.VisualStudio.Web.CodeGeneration.Design` from 8.0.7 to 10.0.2
2. **Remove**: `Microsoft.AspNetCore.Authentication.AzureAD.UI` (v6.0.36) - obsolete package
3. **Keep**: All other 9 packages remain at current versions (compatible with .NET 10)

**Note**: The `Microsoft.AspNetCore.Authentication.AzureAD.UI` package is obsolete and was replaced by `Microsoft.Identity.Web` in .NET 6+. Since the project already has `Microsoft.Identity.Web` v3.8.3 (compatible with .NET 10), simply remove the obsolete package reference. Verify authentication code uses Microsoft.Identity.Web APIs.

##### 4. Expected Breaking Changes

See [Breaking Changes Catalog](#breaking-changes-catalog) for complete details.

**Primary Issues**:

**Configuration Binding Changes** (11 occurrences):
- **API**: `IConfiguration.Get<T>()` and `IConfiguration.GetValue<T>()`
- **Issue**: Binary incompatible in .NET 9+ (binding behavior changed)
- **Fix Required**: Add binding options or use alternative patterns
- **Files Affected**: 4 code files (identified in assessment)

**Behavioral Change** (1 occurrence):
- **API**: `UseExceptionHandler(string errorHandlingPath)`
- **Issue**: Behavior changed in .NET 9+
- **Fix Required**: Verify exception handling behavior matches expectations

##### 5. Code Modifications

**Configuration Binding Pattern Updates**:

For `IConfiguration.Get<T>()` calls, update to:
```csharp
// Old pattern
var myConfig = configuration.Get<MyConfigClass>();

// New pattern (option 1 - recommended)
var myConfig = configuration.Get<MyConfigClass>(options => 
    options.ErrorOnUnknownConfiguration = true);

// New pattern (option 2 - use Bind)
var myConfig = new MyConfigClass();
configuration.Bind(myConfig);
```

For `IConfiguration.GetValue<T>()` calls, update to:
```csharp
// Old pattern
var value = configuration.GetValue<int>("SomeKey");

// New pattern
var value = configuration.GetSection("SomeKey").Get<int>();
// OR
var value = int.Parse(configuration["SomeKey"] ?? "0");
```

For `services.Configure<TOptions>(configuration)` calls, the API signature changed but usage typically remains the same. Verify compilation and adjust if needed.

**Exception Handling Validation**:

Locate and review the `UseExceptionHandler` call (typically in `Program.cs` or `Startup.cs`):
```csharp
app.UseExceptionHandler("/Error");
```

Validate that error handling behavior works as expected. The behavioral change is documented in [Breaking Changes Catalog](#breaking-changes-catalog).

**Authentication Code Review**:

Since removing `Microsoft.AspNetCore.Authentication.AzureAD.UI`:
1. Ensure all authentication setup uses `Microsoft.Identity.Web` APIs
2. Typical patterns should already be in place:
   ```csharp
   services.AddMicrosoftIdentityWebAppAuthentication(configuration, "AzureAd");
   ```
3. Remove any `using Microsoft.AspNetCore.Authentication.AzureAD.UI;` statements
4. Verify authentication middleware is properly configured

##### 6. Areas Requiring Code Review

Based on the project type and typical patterns, review these areas:

**Configuration** (Priority: High):
- `Program.cs` / `Startup.cs`: Configuration service registration and binding
- `appsettings.json` / `appsettings.Development.json`: No changes needed, but verify after upgrade

**Authentication & Authorization** (Priority: High):
- Authentication setup in `Program.cs` / `Startup.cs`
- Any custom authentication handlers or middleware
- Authorization policies
- Pages with `[Authorize]` attributes

**Azure Service Integration** (Priority: Medium):
- Azure Storage Queue initialization (`Azure.Storage.Queues`)
- Cosmos DB client setup (`Microsoft.Azure.Cosmos`)
- Application Insights telemetry (`Microsoft.ApplicationInsights.AspNetCore`)

**Middleware Pipeline** (Priority: Medium):
- Exception handling middleware
- Security headers middleware (`NetEscapades.AspNetCore.SecurityHeaders`)
- Custom middleware components

**Razor Pages** (Priority: Low):
- Page models (should be largely unaffected)
- Dependency injection in constructors
- View components if any

##### 7. Testing Strategy

See [Testing & Validation Strategy](#testing--validation-strategy) for comprehensive testing plan.

**Focus Areas**:
1. **Authentication**: Entra ID (Azure AD) login, logout, authorization policies
2. **Configuration**: All configuration-driven features work correctly
3. **Azure Services**: Storage queue operations, Cosmos DB queries, telemetry
4. **Error Handling**: Application error pages and exception handling
5. **General Functionality**: Core features, navigation, data operations

##### 8. Validation Checklist

- [ ] Project builds without errors
- [ ] Project builds without warnings
- [ ] No obsolete API warnings
- [ ] All unit tests pass (if present)
- [ ] Authentication flows work (login, logout, protected pages)
- [ ] Azure service connections operational
- [ ] Application Insights telemetry flowing
- [ ] Error handling displays correctly
- [ ] All major features functional
- [ ] No console errors or warnings in browser
- [ ] Performance is acceptable

---

## Package Update Reference

### Summary

- **Total Packages**: 11
- **Packages to Update**: 1
- **Packages to Remove**: 1
- **Packages Unchanged**: 9 (already compatible with .NET 10)

### Package Updates

| Package | Current Version | Target Version | Update Reason | Projects Affected |
| :--- | :---: | :---: | :--- | :--- |
| Microsoft.VisualStudio.Web.CodeGeneration.Design | 8.0.7 | 10.0.2 | Framework alignment - required for .NET 10 tooling | robert-brands-com.csproj |

### Package Removals

| Package | Current Version | Reason | Replacement |
| :--- | :---: | :--- | :--- |
| Microsoft.AspNetCore.Authentication.AzureAD.UI | 6.0.36 | Obsolete - deprecated since .NET 6 | Already using Microsoft.Identity.Web v3.8.3 |

**Removal Notes**:
- This package was superseded by `Microsoft.Identity.Web` in .NET 6
- The project already has `Microsoft.Identity.Web` v3.8.3 installed
- Simply remove the obsolete package reference
- Verify authentication code uses `Microsoft.Identity.Web` APIs (should already be the case)
- No additional package installation required

### Compatible Packages (No Changes Required)

These packages are compatible with .NET 10.0 and require no version changes:

| Package | Current Version | Status | Notes |
| :--- | :---: | :--- | :--- |
| Azure.Storage.Queues | 12.22.0 | ✅ Compatible | Azure SDK for .NET supports .NET 10 |
| Flurl | 4.0.0 | ✅ Compatible | Targets netstandard2.0+ |
| Flurl.Http | 4.0.2 | ✅ Compatible | Targets netstandard2.0+ |
| Ical.Net | 4.3.1 | ✅ Compatible | Targets netstandard2.0+ |
| Microsoft.ApplicationInsights.AspNetCore | 2.23.0 | ✅ Compatible | Supports .NET 6+ |
| Microsoft.Azure.Cosmos | 3.48.1 | ✅ Compatible | Azure SDK supports .NET 10 |
| Microsoft.Identity.Web | 3.8.3 | ✅ Compatible | Supports .NET 8+ |
| NetEscapades.AspNetCore.SecurityHeaders | 1.0.0 | ✅ Compatible | Targets netstandard2.0+ |
| reCAPTCHA.AspNetCore | 3.0.10 | ✅ Compatible | Targets netstandard2.0+ |

### Package Update Instructions

#### 1. Update Microsoft.VisualStudio.Web.CodeGeneration.Design

**Method 1 - Edit .csproj directly**:
```xml
<!-- Change from: -->
<PackageReference Include="Microsoft.VisualStudio.Web.CodeGeneration.Design" Version="8.0.7" />

<!-- To: -->
<PackageReference Include="Microsoft.VisualStudio.Web.CodeGeneration.Design" Version="10.0.2" />
```

**Method 2 - Using dotnet CLI**:
```bash
dotnet remove package Microsoft.VisualStudio.Web.CodeGeneration.Design
dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design --version 10.0.2
```

**Method 3 - Using NuGet Package Manager**:
- Visual Studio: Right-click project → Manage NuGet Packages → Updates tab → Select package → Update

#### 2. Remove Microsoft.AspNetCore.Authentication.AzureAD.UI

**Method 1 - Edit .csproj directly**:
```xml
<!-- Remove this line entirely: -->
<PackageReference Include="Microsoft.AspNetCore.Authentication.AzureAD.UI" Version="6.0.36" />
```

**Method 2 - Using dotnet CLI**:
```bash
dotnet remove package Microsoft.AspNetCore.Authentication.AzureAD.UI
```

**Method 3 - Using NuGet Package Manager**:
- Visual Studio: Right-click project → Manage NuGet Packages → Installed tab → Select package → Uninstall

#### 3. Restore Packages

After making changes:
```bash
dotnet restore
```

Or in Visual Studio: Build → Restore NuGet Packages

### Package Update Validation

After updating packages:
- [ ] `dotnet restore` completes without errors
- [ ] No package version conflicts reported
- [ ] No missing package warnings
- [ ] Project builds successfully

---

## Breaking Changes Catalog

This section documents all known breaking changes affecting this upgrade from .NET 8.0 to .NET 10.0.

### Overview

| Category | Count | Severity |
| :--- | :---: | :--- |
| Binary Incompatible APIs | 11 | 🔴 High - Code changes required |
| Behavioral Changes | 1 | 🟡 Medium - Validation required |
| Source Incompatible | 0 | N/A |

### Binary Incompatible API Changes

#### 1. Configuration Binding - IConfiguration.Get<T>()

**API**: `Microsoft.Extensions.Configuration.ConfigurationBinder.Get<T>(IConfiguration)`

**Occurrences**: 5 in project

**Issue**: Binary incompatible change in .NET 9+ - method signature changed to include options parameter

**Current Usage**:
```csharp
var config = configuration.Get<MyConfigType>();
```

**Required Fix**:
```csharp
// Option 1: Specify binding options (recommended)
var config = configuration.Get<MyConfigType>(options => 
{
    options.ErrorOnUnknownConfiguration = true;
    options.BindNonPublicProperties = false; // default
});

// Option 2: Use Bind method
var config = new MyConfigType();
configuration.Bind(config);

// Option 3: Use specific options pattern
services.Configure<MyConfigType>(configuration);
var config = serviceProvider.GetRequiredService<IOptions<MyConfigType>>().Value;
```

**Recommendation**: Use Option 1 with `ErrorOnUnknownConfiguration = true` for better validation.

**Files Likely Affected**: 
- Program.cs / Startup.cs (configuration setup)
- Any service registration code using configuration binding

---

#### 2. Configuration Binding - IServiceCollection.Configure<T>()

**API**: `Microsoft.Extensions.DependencyInjection.OptionsConfigurationServiceCollectionExtensions.Configure<T>(IServiceCollection, IConfiguration)`

**Occurrences**: 4 in project

**Issue**: Binary incompatible change in .NET 9+ - extension method signature modified

**Current Usage**:
```csharp
services.Configure<MyOptions>(configuration.GetSection("MyOptions"));
```

**Typical Fix**: In most cases, this API continues to work, but recompilation is required. If compilation fails:

```csharp
// Updated pattern with explicit options
services.Configure<MyOptions>(options => 
    configuration.GetSection("MyOptions").Bind(options));

// Or use AddOptions pattern
services.AddOptions<MyOptions>()
    .Bind(configuration.GetSection("MyOptions"))
    .ValidateDataAnnotations()
    .ValidateOnStart();
```

**Recommendation**: Prefer `AddOptions<T>()` pattern for better validation and configuration.

**Files Likely Affected**:
- Program.cs / Startup.cs (service configuration)

---

#### 3. Configuration Binding - IConfiguration.GetValue<T>()

**API**: `Microsoft.Extensions.Configuration.ConfigurationBinder.GetValue<T>(IConfiguration, string)`

**Occurrences**: 2 in project

**Issue**: Binary incompatible change in .NET 9+

**Current Usage**:
```csharp
var timeout = configuration.GetValue<int>("RequestTimeout");
var isEnabled = configuration.GetValue<bool>("FeatureEnabled");
```

**Required Fix**:
```csharp
// Option 1: Use GetSection().Get<T>()
var timeout = configuration.GetSection("RequestTimeout").Get<int>();
var isEnabled = configuration.GetSection("FeatureEnabled").Get<bool>();

// Option 2: Use indexer with parsing
var timeout = int.Parse(configuration["RequestTimeout"] ?? "30");
var isEnabled = bool.Parse(configuration["FeatureEnabled"] ?? "false");

// Option 3: Use GetValue with default (if overload available)
var timeout = configuration.GetValue("RequestTimeout", defaultValue: 30);
```

**Recommendation**: Use Option 1 for type safety, Option 2 when defaults needed.

**Files Likely Affected**:
- Configuration-dependent service classes
- Middleware initialization code

---

### Behavioral Changes

#### 4. Exception Handling - UseExceptionHandler()

**API**: `Microsoft.AspNetCore.Builder.ExceptionHandlerExtensions.UseExceptionHandler(IApplicationBuilder, string)`

**Occurrences**: 1 in project (typically)

**Issue**: Behavioral change in exception handling in .NET 9+

**Current Usage**:
```csharp
app.UseExceptionHandler("/Error");
```

**Behavioral Change Details**:
- Exception handling pipeline behavior changed in .NET 9+
- Error response generation may differ
- Status code handling may have changed
- Endpoint routing interaction may differ

**Validation Required**:
1. Test application error scenarios
2. Verify error page displays correctly
3. Check that appropriate status codes are returned
4. Validate that custom error responses work as expected
5. Test API error responses (if applicable)

**Potential Fix (if issues found)**:
```csharp
// Option 1: Use new options pattern (if available in .NET 10)
app.UseExceptionHandler(new ExceptionHandlerOptions
{
    ExceptionHandlingPath = "/Error",
    // Additional configuration
});

// Option 2: Custom exception handler middleware
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.StatusCode = 500;
        context.Response.ContentType = "text/html";
        await context.Response.WriteAsync("An error occurred.");
    });
});
```

**Recommendation**: Test thoroughly before and after upgrade to identify any behavioral differences.

**File Likely Affected**:
- Program.cs / Startup.cs (middleware pipeline)

---

### Framework-Level Breaking Changes

#### .NET 8 to .NET 10 General Changes

Review these areas for potential additional breaking changes:

**ASP.NET Core Changes**:
- Razor Pages compilation may have minor differences
- Middleware pipeline ordering may be more strict
- Endpoint routing behavior refinements
- Model binding behavior adjustments

**Configuration System**:
- All configuration binding APIs have evolved (covered above)
- Configuration providers may have new validation
- Options pattern validation is stricter

**Dependency Injection**:
- Service lifetime validation is stricter
- Circular dependency detection improved
- Disposal order may differ

**Authentication**:
- Microsoft.Identity.Web may have behavior changes
- Token validation may be stricter
- Claims transformation may differ

**Minimal Impact Expected**:
- Most changes are evolutionary, not breaking
- Well-structured code should be minimally affected
- Comprehensive testing will catch edge cases

---

### Migration Checklist

After applying code fixes:

- [ ] All `IConfiguration.Get<T>()` calls updated
- [ ] All `IConfiguration.GetValue<T>()` calls updated
- [ ] All `services.Configure<T>()` calls compile correctly
- [ ] `UseExceptionHandler` behavior validated
- [ ] Authentication flows tested
- [ ] Azure service integrations tested
- [ ] Configuration-driven features verified
- [ ] Error handling pages tested
- [ ] Application builds without errors
- [ ] Application builds without warnings
- [ ] All tests pass

---

## Risk Management

### Risk Level Assessment

**Overall Project Risk: 🟢 LOW**

The upgrade carries low risk due to:
- Single project with no dependencies
- Small codebase with minimal changes required
- Well-documented .NET 8 → .NET 10 migration path
- No security vulnerabilities identified
- Modern SDK-style project format

### Specific Risks

| Risk Area | Risk Level | Description | Mitigation |
| :--- | :---: | :--- | :--- |
| Authentication Changes | 🟡 Medium | Removing obsolete AzureAD.UI package; existing Microsoft.Identity.Web implementation must be validated | Review authentication configuration and test all auth flows thoroughly |
| Configuration Binding | 🟡 Medium | 11 binary incompatible API calls in configuration binding methods require code changes | Follow documented patterns for new .NET 10 configuration binding with validation |
| Behavioral Changes | 🟢 Low | UseExceptionHandler has behavior change | Test error handling paths to ensure expected behavior |
| Package Updates | 🟢 Low | Only 1 package needs version update | Update follows standard versioning, low compatibility risk |
| Azure Integrations | 🟢 Low | Azure packages (Storage, Cosmos, App Insights) are compatible | Validate Azure service connections after upgrade |
| Build Issues | 🟢 Low | Small number of affected files | Fix compilation errors systematically following breaking changes documentation |

### Security Considerations

- **No Security Vulnerabilities** identified in current package versions
- **Authentication Modernization**: Removing obsolete package is actually a security improvement
- **LTS Version**: .NET 10.0 is Long Term Support, ensuring long-term security updates

### Contingency Plans

**If Authentication Issues Arise**:
- Review Microsoft.Identity.Web migration guide
- Ensure proper configuration in appsettings.json and Startup/Program.cs
- Validate Entra ID app registration settings
- Test with both authenticated and anonymous access patterns

**If Configuration Binding Fails**:
- Use new `Get<T>(options => options.BindNonPublicProperties = true)` pattern where needed
- Replace `GetValue<T>` with `GetSection().Get<T>()`
- Apply `[Required]` attributes for validation
- Consider using `IOptions<T>` pattern for complex configurations

**If Build Errors Persist**:
- Review .NET 10 breaking changes documentation
- Check for implicit usings that may have changed
- Verify all necessary framework references are present
- Consider enabling nullable reference types warnings

**If Azure Service Issues**:
- Verify connection strings and configuration
- Check for any Azure SDK breaking changes
- Test each Azure service independently
- Review Azure SDK changelogs for .NET 10 compatibility notes

### Rollback Strategy

1. **Git Branch Rollback**: Abandon `upgrade-to-NET10` branch and return to `users/rbrands/2026-03-21`
2. **Commit Revert**: If issues found after merge, revert the upgrade commit(s)
3. **Package Downgrade**: If partial rollback needed, revert packages.lock.json (if present) or manually downgrade packages

**Recovery Time**: < 10 minutes (git branch switch or commit revert)

---

## Testing & Validation Strategy

### Multi-Level Testing Approach

Given the All-At-Once strategy, testing occurs in a single comprehensive phase after all changes are applied.

### Per-Project Testing (During Development)

As code changes are made during the upgrade:

**Build Validation**:
- [ ] Project builds without errors
- [ ] Project builds without warnings
- [ ] No obsolete API usage warnings
- [ ] All package references resolve correctly

**Quick Smoke Test**:
- [ ] Application starts successfully
- [ ] Home page loads
- [ ] No console errors in browser developer tools

### Comprehensive Testing (After All Changes)

#### 1. Configuration Testing

**Objective**: Verify all configuration binding changes work correctly

Test Cases:
- [ ] Application starts with valid configuration
- [ ] Configuration values are read correctly
- [ ] Options pattern configurations are properly bound
- [ ] Invalid configuration is detected (if validation enabled)
- [ ] Configuration from different sources works (appsettings.json, environment variables, Azure App Configuration if used)

**How to Test**:
1. Start application in Development mode
2. Verify all features that depend on configuration work
3. Check application logs for configuration warnings
4. Test with different configuration profiles if applicable

---

#### 2. Authentication & Authorization Testing

**Objective**: Verify Entra ID (Azure AD) authentication works after removing obsolete package

**Critical Test Cases**:
- [ ] Anonymous access to public pages works
- [ ] Login redirect works (redirects to Microsoft login)
- [ ] Successful authentication redirects back to application
- [ ] User identity and claims are available
- [ ] Logout works correctly
- [ ] Protected pages require authentication (redirect to login)
- [ ] Role-based authorization works (Admin, User roles)
- [ ] Unauthorized access returns appropriate error (403)

**Test Scenarios**:

**Public Access**:
1. Navigate to home page without login
2. Verify public content displays
3. Verify navigation works

**Authentication Flow**:
1. Navigate to protected page (e.g., /Admin/ActivityLog)
2. Verify redirect to Microsoft login
3. Login with valid credentials
4. Verify redirect back to requested page
5. Verify user identity in UI (check _LoginPartial display)

**Authorization**:
1. Login as Admin user
2. Verify Admin dropdown menu visible
3. Access admin-only pages
4. Login as regular User
5. Verify admin menu not visible
6. Attempt to access admin page directly (should get 403)

**Logout**:
1. Click logout
2. Verify redirect to home or logout confirmation
3. Verify authentication cookie cleared
4. Attempt to access protected page (should redirect to login)

**Files to Review**:
- Program.cs / Startup.cs: Authentication configuration
- _LoginPartial.cshtml: User display logic
- Pages with [Authorize] attributes

---

#### 3. Azure Services Testing

**Objective**: Verify Azure service integrations continue to work

**Azure Storage Queues** (Azure.Storage.Queues):
- [ ] Queue client initializes successfully
- [ ] Messages can be sent to queue
- [ ] Messages can be received from queue
- [ ] Connection string / managed identity authentication works

**Azure Cosmos DB** (Microsoft.Azure.Cosmos):
- [ ] Cosmos client initializes successfully
- [ ] Database and container access works
- [ ] Read operations succeed
- [ ] Write operations succeed
- [ ] Query operations return expected results

**Application Insights** (Microsoft.ApplicationInsights.AspNetCore):
- [ ] Telemetry initialization succeeds
- [ ] Page views are tracked
- [ ] Custom events are logged (if used)
- [ ] Exceptions are captured
- [ ] Telemetry appears in Azure Portal

**Test Approach**:
1. Execute application features that use each service
2. Check application logs for connection errors
3. Verify operations succeed
4. Check Azure Portal for telemetry data (Application Insights)

---

#### 4. Error Handling Testing

**Objective**: Verify UseExceptionHandler behavioral change doesn't break error handling

Test Cases:
- [ ] Unhandled exceptions show error page
- [ ] Error page displays correctly (/Error page)
- [ ] Appropriate HTTP status code returned (500)
- [ ] Sensitive information not leaked in error response
- [ ] Custom error handling works (if implemented)
- [ ] API error responses appropriate (if API endpoints exist)

**Test Scenarios**:

**Unhandled Exception**:
1. Trigger an unhandled exception (create test endpoint if needed)
2. Verify error page displays
3. Verify 500 status code returned
4. Check browser console for errors

**404 Errors**:
1. Navigate to non-existent page
2. Verify 404 handling works

**Development vs Production**:
1. Test in Development mode (detailed errors)
2. Test in Production mode (generic errors)

---

#### 5. General Functionality Testing

**Core Features**:

Based on navigation in _Layout.cshtml, test these areas:

**Public Features**:
- [ ] Home page and main navigation
- [ ] Freelancer page (/Blog/Artikel/freelancer)
- [ ] Trainer page (/blog/training)
- [ ] Fotograf section (/Fotos/Index)
- [ ] Radfahrer section (/Rad/Ausfahrten)

**Authenticated User Features**:
- [ ] Fotoalbum (/Fotos/Album)
- [ ] Blog (/Blog/Index)
- [ ] Tour Collections (/Rad/Collections)

**Admin Features**:
- [ ] Activity Log (/Admin/ActivityLog)
- [ ] Track management (/Rad/NewTrackLink)
- [ ] Photo links (/Fotos/NewFotoLink)
- [ ] Shortcuts (/Shortcuts/New, /Shortcuts/Index)
- [ ] Categories (/Admin/Kategorien)
- [ ] Environment/Claims (/Admin/Environment)
- [ ] Headers (/Admin/Headers)
- [ ] Event management (/Events/NewEventRegistration, /Events/ListEventRegistrations)

**Test Approach**:
1. Navigate to each section
2. Verify page loads correctly
3. Test core functionality (forms, data display, etc.)
4. Check for console errors
5. Verify data operations (CRUD) if applicable

---

#### 6. Integration Testing

**End-to-End Scenarios**:

Test complete workflows that span multiple features:

**Scenario 1: Admin Content Management**:
1. Login as Admin
2. Create new blog entry or photo link
3. Verify save succeeds
4. View created content
5. Edit content
6. Verify changes saved
7. Delete content (if applicable)

**Scenario 2: User Authentication Journey**:
1. Start as anonymous user
2. Attempt to access protected content
3. Complete login flow
4. Access previously restricted content
5. Navigate through authenticated sections
6. Logout
7. Verify content access revoked

**Scenario 3: Data Operations**:
1. Perform operations that use Azure services
2. Verify data persistence (Cosmos DB)
3. Verify asynchronous operations (Storage Queues)
4. Check telemetry (Application Insights)

---

#### 7. Performance Validation

**Objective**: Ensure performance is acceptable after upgrade

Quick Checks:
- [ ] Application startup time reasonable
- [ ] Page load times acceptable
- [ ] No obvious performance degradation
- [ ] Memory usage normal
- [ ] No excessive logging or warnings

**If Performance Issues**:
- Review middleware pipeline order
- Check for inefficient configuration binding
- Verify database queries haven't changed
- Check for new framework overhead

---

#### 8. Browser Compatibility

**Objective**: Verify client-side functionality works

Test Browsers:
- [ ] Chrome/Edge (Chromium)
- [ ] Firefox
- [ ] Safari (if applicable)

Quick Checks:
- [ ] Pages render correctly
- [ ] JavaScript works (Bootstrap, jQuery, custom JS)
- [ ] CSS styles apply correctly
- [ ] Forms submit properly
- [ ] No console errors

---

### Testing Environment Checklist

**Development Environment**:
- [ ] .NET 10 SDK installed
- [ ] Valid Azure connection strings in user secrets or appsettings.Development.json
- [ ] Entra ID app registration configured for localhost
- [ ] Test user accounts available

**Configuration**:
- [ ] appsettings.json valid
- [ ] appsettings.Development.json valid
- [ ] User secrets configured (if used)
- [ ] Environment variables set (if needed)

---

### Test Execution Order

1. **Build & Start** → Verify application compiles and starts
2. **Configuration** → Test configuration loading and binding
3. **Authentication** → Complete auth flow tests
4. **Azure Services** → Validate service connections
5. **Core Features** → Test main application functionality
6. **Error Handling** → Verify error pages and exception handling
7. **Integration** → Run end-to-end scenarios
8. **Performance** → Quick performance check
9. **Browser** → Cross-browser validation

---

### Success Criteria for Testing

All tests must pass before upgrade is considered complete:
- ✅ Application builds without errors or warnings
- ✅ All manual test cases pass
- ✅ No regression in functionality
- ✅ Authentication and authorization work correctly
- ✅ Azure service integrations operational
- ✅ Error handling behaves as expected
- ✅ No performance degradation observed

---

### Issue Tracking

If issues found during testing:

1. **Document Issue**: Describe behavior, expected vs actual
2. **Categorize**: Configuration, authentication, API, performance, etc.
3. **Reference Breaking Changes**: Check if covered in Breaking Changes Catalog
4. **Apply Fix**: Use patterns from this plan
5. **Retest**: Verify fix resolves issue
6. **Update Documentation**: If new pattern discovered

---

## Complexity & Effort Assessment

### Project Complexity Rating

| Project | Complexity | LOC | Dependencies | Risk | Rationale |
| :--- | :---: | :---: | :---: | :---: | :--- |
| robert-brands-com.csproj | 🟢 Low | 8,151 | 0 projects, 11 packages | Low | Single ASP.NET Core project, minimal changes, clear upgrade path |

### Phase Complexity Assessment

**Single Atomic Phase**: 🟢 Low Complexity

- ✅ No dependency coordination required
- ✅ Only 4 code files need changes (~4% of total files)
- ✅ Estimated 12+ LOC modifications (~0.1% of codebase)
- ✅ Standard package updates
- ✅ Well-documented API migration patterns

### Effort Distribution

| Activity | Relative Effort | Focus Areas |
| :--- | :---: | :--- |
| Project File Updates | Low | Single TargetFramework property change |
| Package Updates | Low | 1 version update, 1 removal |
| API Compatibility Fixes | Medium | 12 API calls (11 binary incompatible, 1 behavioral) |
| Build Validation | Low | Single project build |
| Testing | Medium | Authentication flows, Azure integrations, general functionality |
| Documentation | Low | Update deployment docs if framework dependencies noted |

### Resource Requirements

**Technical Skills Required**:
- .NET/C# development experience
- ASP.NET Core Razor Pages knowledge
- Understanding of configuration patterns in .NET
- Familiarity with Microsoft.Identity.Web (for authentication validation)
- Azure services knowledge (for integration testing)

**Parallel Capacity**: Not applicable (single project, atomic upgrade)

**Estimated Relative Complexity**: **Low** - Straightforward upgrade with clear migration path and minimal code changes.

---

## Source Control Strategy

### Overview

This upgrade follows an **atomic commit strategy** - all changes applied in a single coordinated operation on a dedicated feature branch.

### Branch Structure

**Source Branch**: `users/rbrands/2026-03-21`
- Starting point for upgrade
- Contains current .NET 8 implementation

**Upgrade Branch**: `upgrade-to-NET10`
- Dedicated branch for all .NET 10 upgrade changes
- Created from source branch
- Contains all framework, package, and code changes

**Target Branch**: `users/rbrands/2026-03-21` (or main/master as appropriate)
- Merge destination after upgrade validated

### Branching Strategy

**Before Starting Upgrade**:
```bash
# Ensure on source branch
git checkout users/rbrands/2026-03-21

# Commit any pending changes
git add .
git commit -m "Pre-upgrade checkpoint"

# Create upgrade branch
git checkout -b upgrade-to-NET10
```

### Commit Strategy

**Single Atomic Commit (Recommended)**:

All upgrade changes in one logical commit for easy rollback:

```bash
# After all changes complete and tested
git add .
git commit -m "Upgrade to .NET 10.0

- Updated TargetFramework from net8.0 to net10.0
- Updated Microsoft.VisualStudio.Web.CodeGeneration.Design to 10.0.2
- Removed obsolete Microsoft.AspNetCore.Authentication.AzureAD.UI package
- Fixed 11 binary incompatible configuration binding API calls
- Validated UseExceptionHandler behavioral change
- All tests passing"
```

**Alternative: Logical Grouping (If Preferred)**:

If breaking into multiple commits, use clear logical boundaries:

```bash
# Commit 1: Project and package changes
git add robert-brands-com.csproj
git commit -m "Update project to .NET 10.0 and update packages"

# Commit 2: Configuration binding fixes
git add Program.cs Startup.cs # (or affected files)
git commit -m "Fix configuration binding API compatibility"

# Commit 3: Additional code fixes
git add <affected files>
git commit -m "Address remaining API breaking changes"

# Commit 4: Validation updates
git add <test-related files>
git commit -m "Update tests and validation"
```

### Commit Message Format

Use clear, descriptive commit messages:

**Format**:
```
<Summary Line>

<Detailed Description>
- Bullet point changes
- Breaking change resolutions
- Package updates
```

**Example**:
```
Upgrade to .NET 10.0 LTS

Updated application from .NET 8.0 to .NET 10.0 Long Term Support version.

Changes:
- Project file: net8.0 → net10.0
- Package updates:
  - Microsoft.VisualStudio.Web.CodeGeneration.Design: 8.0.7 → 10.0.2
  - Removed: Microsoft.AspNetCore.Authentication.AzureAD.UI (obsolete)
- API compatibility fixes:
  - Updated IConfiguration.Get<T>() calls with binding options
  - Updated IConfiguration.GetValue<T>() to GetSection().Get<T>()
  - Validated services.Configure<T>() calls
  - Tested UseExceptionHandler behavioral change
- Testing:
  - All authentication flows validated
  - Azure service integrations tested
  - Core functionality verified
  - No regressions identified

Closes #<issue-number> (if applicable)
```

### Review and Merge Process

**Pre-Merge Checklist**:
- [ ] All code changes committed
- [ ] Application builds without errors or warnings
- [ ] All tests pass
- [ ] Comprehensive manual testing completed
- [ ] No regressions identified
- [ ] Documentation updated (if needed)
- [ ] Commit messages are clear and descriptive

**Pull Request (If Using PR Workflow)**:

Create PR from `upgrade-to-NET10` to target branch:

**PR Title**: `Upgrade to .NET 10.0 LTS`

**PR Description Template**:
```markdown
## Overview
Upgrades robert-brands-com from .NET 8.0 to .NET 10.0 (Long Term Support).

## Changes
- **Project**: Updated TargetFramework to net10.0
- **Packages**: 
  - Updated: Microsoft.VisualStudio.Web.CodeGeneration.Design (8.0.7 → 10.0.2)
  - Removed: Microsoft.AspNetCore.Authentication.AzureAD.UI (obsolete)
- **Code**: Fixed 12 API compatibility issues (configuration binding, exception handler)

## Testing Performed
- [x] Build succeeds with no errors/warnings
- [x] Authentication flows tested (login, logout, authorization)
- [x] Azure service integrations validated
- [x] Error handling tested
- [x] Core functionality verified
- [x] No performance degradation

## Breaking Changes
- Configuration binding APIs updated to .NET 9+ patterns
- UseExceptionHandler behavior validated

## Rollback Plan
Revert to branch `users/rbrands/2026-03-21` if issues arise.

## Related Issues
Closes #<issue-number> (if applicable)
```

**Review Criteria**:
- Code changes align with breaking changes documentation
- All affected areas tested
- No unnecessary changes included
- Commit history clean and logical

**Merge Method**:
- **Squash Merge**: If multiple commits, squash to single commit on target branch
- **Regular Merge**: If single atomic commit, use regular merge to preserve history

### Post-Merge Actions

After successful merge:

```bash
# Switch to target branch
git checkout users/rbrands/2026-03-21

# Pull merged changes
git pull origin users/rbrands/2026-03-21

# Optionally delete upgrade branch
git branch -d upgrade-to-NET10
git push origin --delete upgrade-to-NET10
```

### Rollback Procedures

**If Issues Found Before Merge**:
```bash
# Abandon upgrade branch
git checkout users/rbrands/2026-03-21

# Optionally delete upgrade branch
git branch -D upgrade-to-NET10
```

**If Issues Found After Merge**:
```bash
# Revert the merge commit
git revert -m 1 <merge-commit-hash>

# Or reset to pre-merge state (if not pushed)
git reset --hard <pre-merge-commit-hash>
```

### Continuous Integration Considerations

If using CI/CD pipeline:

**Branch Policies**:
- Require successful build on `upgrade-to-NET10` branch
- Require passing tests
- Require code review (if team environment)

**Pipeline Updates** (if needed):
- Update build agents to have .NET 10 SDK
- Update deployment scripts for .NET 10 runtime
- Update Docker base images (if containerized)

### Tag Strategy

After successful merge and validation:

```bash
# Create version tag
git tag -a v<version>-net10 -m "Upgraded to .NET 10.0"

# Example
git tag -a v2.0-net10 -m "Upgraded to .NET 10.0 LTS"

# Push tag
git push origin v<version>-net10
```

---

## Success Criteria

### Overview

The .NET 10.0 upgrade is considered successful when ALL of the following criteria are met.

---

### Technical Criteria

#### Build & Compilation

- [x] **Target Framework Updated**: Project targets net10.0
- [ ] **Build Succeeds**: `dotnet build` completes without errors
- [ ] **No Build Warnings**: Solution builds clean (0 warnings related to upgrade)
- [ ] **No Obsolete API Warnings**: No warnings about deprecated APIs
- [ ] **Package Restore Succeeds**: `dotnet restore` completes without errors
- [ ] **No Package Conflicts**: All package dependencies resolve correctly

#### Package Management

- [ ] **Code Generation Tool Updated**: Microsoft.VisualStudio.Web.CodeGeneration.Design is version 10.0.2
- [ ] **Obsolete Package Removed**: Microsoft.AspNetCore.Authentication.AzureAD.UI no longer referenced
- [ ] **Compatible Packages Verified**: All 9 remaining packages work with .NET 10
- [ ] **No Security Vulnerabilities**: No vulnerable packages reported

#### API Compatibility

- [ ] **Configuration Binding Fixed**: All 11 binary incompatible API calls resolved
  - [ ] `IConfiguration.Get<T>()` calls updated (5 occurrences)
  - [ ] `IServiceCollection.Configure<T>()` calls updated (4 occurrences)
  - [ ] `IConfiguration.GetValue<T>()` calls updated (2 occurrences)
- [ ] **Behavioral Changes Validated**: UseExceptionHandler behavior verified
- [ ] **No Runtime Errors**: Application runs without framework-related exceptions

---

### Quality Criteria

#### Code Quality

- [ ] **Code Compiles**: All C# code compiles successfully
- [ ] **No Runtime Exceptions**: No framework or compatibility exceptions during testing
- [ ] **Configuration Loads**: Application configuration loads correctly
- [ ] **Best Practices Applied**: Updated code follows .NET 10 recommended patterns
- [ ] **Error Handling Works**: Exception handling behaves as expected

#### Testing & Validation

**Unit Tests** (if present):
- [ ] All existing unit tests pass
- [ ] No tests skipped or disabled due to upgrade
- [ ] Test frameworks compatible with .NET 10

**Manual Testing**:
- [ ] **Authentication**: All authentication flows work (login, logout, protected pages)
- [ ] **Authorization**: Role-based access control works (Admin, User roles)
- [ ] **Configuration**: All configuration-driven features work
- [ ] **Azure Storage**: Queue operations succeed
- [ ] **Azure Cosmos DB**: Database operations succeed
- [ ] **Application Insights**: Telemetry flows correctly
- [ ] **Error Pages**: Error handling displays correctly
- [ ] **Core Features**: All major application features function correctly

**Performance**:
- [ ] Application startup time is acceptable
- [ ] Page load times are similar or better than .NET 8
- [ ] No obvious performance degradation
- [ ] Memory usage is normal

**Browser Compatibility**:
- [ ] Application works in Chrome/Edge
- [ ] Application works in Firefox
- [ ] No console errors in browser developer tools

#### Documentation

- [ ] **README Updated**: If README mentions .NET version, it's updated to .NET 10
- [ ] **Deployment Docs**: Deployment documentation updated if framework dependency noted
- [ ] **CI/CD**: Build pipeline updated for .NET 10 SDK (if applicable)
- [ ] **Docker**: Dockerfile/Docker Compose updated for .NET 10 runtime (if applicable)

---

### Process Criteria

#### All-At-Once Strategy Execution

- [ ] **Single Atomic Operation**: All changes applied in coordinated manner
- [ ] **No Intermediate States**: Direct upgrade from .NET 8 to .NET 10
- [ ] **Framework + Packages + Code**: All aspects updated together
- [ ] **Single Testing Pass**: Comprehensive validation after all changes

#### Source Control

- [ ] **Dedicated Branch**: All changes on `upgrade-to-NET10` branch
- [ ] **Atomic Commits**: Changes committed logically (single commit or logical grouping)
- [ ] **Clear Commit Messages**: Descriptive messages following format in plan
- [ ] **Review Completed**: Code review done (if team environment)
- [ ] **Merged to Target**: Changes merged to target branch after validation

#### Risk Management

- [ ] **All Risks Addressed**: Known risks from Risk Management section mitigated
- [ ] **Authentication Validated**: Microsoft.Identity.Web integration confirmed working
- [ ] **Breaking Changes Resolved**: All items in Breaking Changes Catalog addressed
- [ ] **Rollback Available**: Can revert to .NET 8 if needed

---

### Functional Acceptance Criteria

#### Core Application Features

Based on _Layout.cshtml navigation structure:

**Public Features** (No Authentication):
- [ ] Home page loads
- [ ] Freelancer page works (/Blog/Artikel/freelancer)
- [ ] Trainer page works (/blog/training)
- [ ] Fotograf section loads (/Fotos/Index)
- [ ] Radfahrer section loads (/Rad/Ausfahrten)

**Authenticated User Features**:
- [ ] Fotoalbum accessible (/Fotos/Album)
- [ ] Blog accessible (/Blog/Index)
- [ ] Tour Collections accessible (/Rad/Collections)

**Admin Features**:
- [ ] Activity Log works (/Admin/ActivityLog)
- [ ] Track management works (/Rad/NewTrackLink)
- [ ] Photo link management works (/Fotos/NewFotoLink)
- [ ] Shortcuts management works (/Shortcuts/New, /Shortcuts/Index)
- [ ] Categories management works (/Admin/Kategorien)
- [ ] Environment/Claims page works (/Admin/Environment)
- [ ] Headers page works (/Admin/Headers)
- [ ] Event management works (/Events/NewEventRegistration, /Events/ListEventRegistrations)

#### Integration Features

- [ ] **Entra ID Authentication**: Login with Microsoft account works
- [ ] **Azure Storage Queues**: Queue operations complete successfully
- [ ] **Azure Cosmos DB**: Database reads and writes work
- [ ] **Application Insights**: Telemetry appears in Azure Portal
- [ ] **reCAPTCHA**: Form validation works (if used)
- [ ] **Security Headers**: Custom headers applied correctly

---

### Deployment Readiness

#### Pre-Deployment Validation

- [ ] Application runs in Development environment
- [ ] Application runs in Staging environment (if available)
- [ ] Configuration validated for Production environment
- [ ] Azure services connection strings verified
- [ ] Entra ID app registration configured for production URLs

#### Deployment Prerequisites

- [ ] **.NET 10 Runtime**: Available in target deployment environment
- [ ] **Azure App Service**: Supports .NET 10 (if using App Service)
- [ ] **Container Images**: Updated to .NET 10 base images (if containerized)
- [ ] **CI/CD Pipeline**: Updated to use .NET 10 SDK
- [ ] **Monitoring**: Application Insights ready to receive .NET 10 telemetry

---

### Sign-Off Checklist

Final verification before considering upgrade complete:

#### Developer Sign-Off
- [ ] All code changes implemented
- [ ] All compilation errors resolved
- [ ] All known issues addressed
- [ ] Code follows .NET 10 best practices
- [ ] Documentation updated

#### Quality Assurance Sign-Off
- [ ] Comprehensive testing completed
- [ ] No critical or high-priority bugs
- [ ] Performance validated
- [ ] Security validated

#### Operational Sign-Off
- [ ] Deployment plan reviewed
- [ ] Rollback plan tested
- [ ] Monitoring configured
- [ ] Runbook updated (if applicable)

---

### Definition of Done

**The upgrade is COMPLETE when:**

1. ✅ All Technical Criteria met
2. ✅ All Quality Criteria met
3. ✅ All Process Criteria met
4. ✅ All Functional Acceptance Criteria met
5. ✅ Deployment Readiness confirmed
6. ✅ All stakeholder sign-offs obtained

**Post-Upgrade**:
- Monitor application for 24-48 hours in production
- Review Application Insights for any anomalies
- Collect user feedback
- Document any lessons learned

---

### Success Metrics

**Upgrade Success Indicators**:
- Zero runtime errors related to framework upgrade
- No performance degradation (or performance improvement)
- All features functional
- No user-reported issues related to upgrade
- Application Insights telemetry flowing correctly

**Long-Term Success**:
- Application stable on .NET 10 for extended period
- Security updates applied regularly
- Performance maintained or improved
- Developer experience positive
- Maintenance simplified by LTS support
