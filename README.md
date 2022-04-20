# kConvert
convert wma files to mp3 including id3 tags

Just came across some older wma files created on Windows using Windows Media Player. As an iPhone won't play those files directly, we tried to convert them to good old mp3 format. Unfortunately the available tools didn't treat metadata carefully enough. Most apps simple forgot to tag the mp3 files. So this two-hour-project started.

Feel free to use kConvert for your files. Feel free to improve and extend the code if needed.

## Usage
run kConvert in command line:
`kConvert <source folder> <optional:destination folder>`

kConvert looks for .wma files in given source folder and converts them to .mp3 files in either the same folder or the destination folder. Metadata is copied from source file after the mp3 files has been generated.
Currently no transcoding etc. is done. Mp3 files are LAME standard quality.

## Build
- clone this repository
- use Visual Studio 2019 to open the solution
- make sure all dependencies are fullfilled using nuget
- build and run

## Dependencies
several external packages are used:
- [NAudio 2.0.1](https://www.nuget.org/packages/NAudio/) including NAudio.Lame
- [NLog 4.7.15](https://www.nuget.org/packages/NLog/4.7.15)
- [TagLibSharp 2.2.0](https://www.nuget.org/packages/TagLibSharp/)

## Description – How does it work
...
