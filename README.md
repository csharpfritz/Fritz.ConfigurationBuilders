# Fritz.ConfigurationBuilders
A collection of simple but useful configurationbuilders for .NET 4.7.1 and later.  You can install these features from [NuGet](https://www.nuget.org/packages/Fritz.ConfigurationBuilders/0.2.0-preview) by executing:

`Install-Package Fritz.ConfigurationBuilders`

Big thanks to AppVeyor for building our open source project:

Build Status:  [![Build status](https://ci.appveyor.com/api/projects/status/3wrl63fly957bc80?svg=true)](https://ci.appveyor.com/project/csharpfritz/fritz-configurationbuilders)

## Contents

This project contains ConfigurationBuilders for your application to be able to consume and work with:

*  INI Files

You can add this library to your project and start consuming files and configuration data from these sources using the .NET Framework's standard `System.Configuration.ConfigurationManager` interface.

## How to use 

### Requirements

*  You must be using at least .NET Framework 4.7.1
*  You must be targeting Windows (kind of required for Net471)
*  You must have a `web.config` or `app.config` in your project

### Configuration File Updates

First, you need to declare the ConfigurationBuilders section in your project file.  This is required for any configurationbuilder, not just the ones included here.  Add the following section code to your project:


```xml
<configSections>
  <section name="configBuilders"
    type="System.Configuration.ConfigurationBuildersSection, System.Configuration"/>
</configSections>
```

You could set the sectionName to anything you would like, but for ease of this demonstration, we're using `configBuilders`.  You next need to add the section called `configBuilders` that defines the actual ConfigurationBuilders with their configuration settings to use:

```xml
<configBuilders>
  <builders>
    <add name="Ini" 
         type="Fritz.ConfigurationBuilders.IniConfigurationBuilder, Fritz.ConfigurationBuilders"
         location="config.ini" />
  </builders>
</configBuilders>
```
The top-level configBuilders element is the same tag-name as the section name from the previous code sample. This section contains a single element, `builders` that contains `add`, `remove`, and `clear` elements.  You use the `add` element to define new ConfigurationBuilder implementations to use in this file.  

The add element requires `name` and `type` attributes.  The `name` attribute can be set to any value you would like and will be referenced using this `name` later in this configuration file.  The `type` attribute value is the fully qualified name of the ConfigurationBuilder that you wish to use.  In this case, we are referencing the `IniConfigurationBuilder` by using the full namespace for that class, followed by the assembly name the class resides in.

Different ConfigurationBuilders have different attribute requirements in order to use them.  In this case, the `location` attribute is required in order to use the `IniConfigurationBuilder`.  More information about how to use each of the ConfigurationBuilders is available on the [Wiki](https://github.com/csharpfritz/Fritz.ConfigurationBuilders/wiki)

To load configuration into a section of your web.config or app.config file, you simply add an attribute to that section that indicates which ConfigurationBuilders to use.

```xml
<appSettings configBuilders="Ini">
</appSettings>
```
