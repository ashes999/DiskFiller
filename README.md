# Disk Filler

Have you ever needed to test what happens to your applications when they run out of space? Now, you can!

Disk Filler quickly fills up the target folder with files containing random alphanumeric content. It's fast, creating about one gigabyte of data per minute.

# Installation

- Make sure .NET 4.0 is installed
- Unpack the binaries, preferably wherever you want Disk Filler to create files
- Run the executable

# Usage

**Always run Disk Filler in a a safe, preferably virtualized environment.** Bad Things can happen to Windows when your primary drive runs out of space.

Disk Filler creates one file per CPU, and appends to it indefinitely. If you want more, smaller files, just hit stop and start again, and it'll create some new files.
