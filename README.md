# MySCSSRepo

This is a temporary repository to contain SCSS's IntegrationApi nuget packages and sample client.

## Demo Client

The `IAPI.Fis.Demo` folder contains a minimal example of a console application that uses the API to synchronize one or more users to SCView. This is primarily a test harness to ensure the client functions within CSIU's .NET Framework and dependency limitations.  

> Note: Financial DLL's must be provided in order to execute the demo.

## Nuget Packages

Two nuget packages are required to use the IAPI Client:

* SCSS.IntegrationApi.Model
* SCSS.IntegrationApi.Client

## Nuget Repo

SCSS is in the process of establishing a public Nuget repostory.  This repository will be used until the public repo is available. 

To reference the nuget packages from this repo:

* Add a package source: https://nuget.pkg.github.com/scss-dsmith/index.json
* Username: scss-dsmith
* Password:  <<contact dsmith@scview.com for temporary access token>>

For example, create this `nuget.config` in the solution directory:

```xml
<configuration>
  <packageSources>
    <add key="scsspackages" value="https://nuget.pkg.github.com/scss-dsmith/index.json" />
  </packageSources>
  <packageSourceCredentials>
    <github>
      <add key="Username" value="scss-dsmith" />
      <add key="ClearTextPassword" value="==personal-access-token==" />
    </github>
  </packageSourceCredentials>
</configuration>
```
