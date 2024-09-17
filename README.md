![Icon](AppIcon.png) 
# Mutexy



## v1.0.0.0 - September 2024
**Dependencies**

| Assembly | Version |
| ---- | ---- |
| NET Core | 6.0 (LTS) |

- A multi-threaded resource access demonstration.
- It's easy to block one external thread, but what about many threads? You could create a [Monitor](https://learn.microsoft.com/en-us/dotnet/api/system.threading.monitor?view=net-6.0) system to solve this problem, but I've opted for the simplier solution via [Mutex](https://learn.microsoft.com/en-us/dotnet/api/system.threading.mutex?view=net-6.0)

