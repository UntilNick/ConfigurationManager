# Configuration Manager

Simple library for managing application settings.<br/>
Settings are being saved in JSON format.<br/>
This library depends on [Newtonsoft.Json](https://www.nuget.org/packages/Newtonsoft.Json/) packet.

# Get NuGet package

NuGet package will be available after versioning support update.

# Examples

**Simple config creation:**

```CSharp
var applicationSettings = new ApplicationSettings("./config.json");

if(applicationSettings.LoadConfigCreateIfNeeded()) {
	applicationSettings.SetConfigEntry<string>("main.hello_world", "Hello world!");
	applicationSettings.SetConfigEntry<double>("main.version", 0.1d);
	applicationSettings.SaveConfig();
}
```

**Creating config with default values:**

```CSharp
var applicationSettings = new ApplicationSettings("./config.json");
var success = applicationSettings.LoadConfigCreateIfNeeded(
	Dictionary<string, object>() {
		{"executable_path", null},
		{"magicGlobalCounter", 0}});

if(success) {
	//continue
}
```
