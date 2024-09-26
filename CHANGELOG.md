# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/en/1.0.0/)
and this project adheres to [Semantic Versioning](http://semver.org/spec/v2.0.0.html).

## [1.8.0] / 2024-09-26
### Features
- Support `IJsonService` to use `NewtonsoftJsonService` if available.
### Updated
- Update namespace to `ricaun.NamedPipeWrapper`.
- Update package to `ricaun.NamedPipeWrapper.Json`.
### Removed
- Remove `Newtonsoft.Json` reference.

## [1.7.0] / 2024-01-16
### Features
- Add `net8.0` support.
### Added
- Add `JsonFormatter` with `IJsonFormatter`.
### Removed
- Remove `BinaryFormatter` reference in `Serializable` class.

## [1.6.0] / 2023-11-19
### Features
- Add `net6.0-windows` support.
### Tests
- Add `net6.0-windows` support.

## [1.5.3] / 2023-02-28
### Features
- Add `Connected` event in `NamedPipeClient`
### Tests
- Add `StringTests` with `Connected` and `Disconnected` test.

## [1.5.2] / 2023-02-28
### Fixed
- Remove `Console` in the `JsonExtension`

## [1.5.1] / 2023-02-23
### Features
- Add `Serializable` do not use `JsonUtils`
- Add `Serializable` Test

## [1.5.0] / 2023-01-23
### Features
- Add `JsonUtils` and `JsonExtension`
- Add `Build` project
- Update `Test` project

[vNext]: ../../compare/1.5.0...HEAD
[1.8.0]: ../../compare/1.7.0...1.8.0
[1.7.0]: ../../compare/1.6.0...1.7.0
[1.6.0]: ../../compare/1.5.3...1.6.0
[1.5.3]: ../../compare/1.5.2...1.5.3
[1.5.2]: ../../compare/1.5.1...1.5.2
[1.5.1]: ../../compare/1.5.0...1.5.1
[1.5.0]: ../../compare/1.5.0