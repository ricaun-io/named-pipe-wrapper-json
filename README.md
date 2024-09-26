# ricaun.NamedPipeWrapper.Json

[![Visual Studio 2022](https://img.shields.io/badge/Visual%20Studio-2022-blue)](https://github.com/ricaun-io/named-pipe-wrapper-json)
[![Nuke](https://img.shields.io/badge/Nuke-Build-blue)](https://nuke.build/)
[![License MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)
[![Build](https://github.com/ricaun-io/named-pipe-wrapper-json/actions/workflows/Build.yml/badge.svg)](https://github.com/ricaun-io/named-pipe-wrapper-json/actions)
[![nuget](https://img.shields.io/nuget/v/ricaun.NamedPipeWrapper.Json?logo=nuget&label=nuget&color=blue)](https://www.nuget.org/packages/ricaun.NamedPipeWrapper.Json)

Named Pipe Wrapper Json for .NET Framework 4.0 and .NET Core 6.0.

***This project is based of Andrew C. Dvorak's work at [Named Pipe Wrapper](https://github.com/acdvorak/named-pipe-wrapper), the code updated to support NET Core and json.***

## PackageReference

```xml
<PackageReference Include="ricaun.NamedPipeWrapper.Json" Version="*" />
```

## Features

*  Create named pipe servers that can handle multiple client connections simultaneously.
*  Send strongly-typed messages between clients and servers: any serializable .NET object can be sent over a pipe and will be automatically serialized/deserialized, including cyclical references and complex object graphs.
*  Messages are sent and received asynchronously on a separate background thread and marshalled back to the calling thread (typically the UI).
*  Supports large messages - up to 300 MiB.
*  Design to work inside Autodesk Revit without conflict.

## Json

By default, all classes gonna be serialized/deserialized by `JsonExtension.JsonService` with the interface `IJsonService`, in the end, a JSON string is passed using the `PipeStream`.

If `Newtonsoft.Json` exists in the project, the `NewtonsoftJsonService` will be used instead of the default `JsonService`. 

The default `JsonService` for NETFRAMWORK use `System.Web.Extensions`, and for NETCOREAPP the `System.Text.Json`.


## Usage

Server:

```csharp
var server = new NamedPipeServer<SomeClass>("MyServerPipe");

server.ClientConnected += delegate(NamedPipeConnection<SomeClass> conn)
    {
        Console.WriteLine("Client {0} is now connected!", conn.Id);
        conn.PushMessage(new SomeClass { Text: "Welcome!" });
    };

server.ClientMessage += delegate(NamedPipeConnection<SomeClass> conn, SomeClass message)
    {
        Console.WriteLine("Client {0} says: {1}", conn.Id, message.Text);
    };

// Start up the server asynchronously and begin listening for connections.
// This method will return immediately while the server runs in a separate background thread.
server.Start();

// ...
```

Client:

```csharp
var client = new NamedPipeClient<SomeClass>("MyServerPipe");

client.ServerMessage += delegate(NamedPipeConnection<SomeClass> conn, SomeClass message)
    {
        Console.WriteLine("Server says: {0}", message.Text);
    };

// Start up the client asynchronously and connect to the specified server pipe.
// This method will return immediately while the client runs in a separate background thread.
client.Start();

// ...
```

## NamedPipeUtils

The `NamedPipeUtils` class provides some utility methods for working with named pipes.

```c#
bool pipeExists = NamedPipeUtils.PipeFileExists("MyServerPipe");
```

## IJsonService

Override the default `JsonService` with the interface `IJsonService`:

```c#
JsonExtension.JsonService = new MyJsonService();
```

## Release

* [Latest release](https://github.com/ricaun-io/ricaun.NamedPipeWrapper.Json/releases/latest)

## License

This project is [licensed](LICENSE) under the [MIT License](https://en.wikipedia.org/wiki/MIT_License).

---

Do you like this project? Please [star this project on GitHub](https://github.com/ricaun-io/ricaun.NamedPipeWrapper.Json/stargazers)!
