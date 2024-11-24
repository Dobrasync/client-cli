<a id="readme-top"></a>

<!-- PROJECT SHIELDS -->
[![Contributors][contributors-shield]][contributors-url]
[![Forks][forks-shield]][forks-url]
[![Stargazers][stars-shield]][stars-url]
[![Issues][issues-shield]][issues-url]
[![GPLv2 License][license-shield]][license-url]

<!-- PROJECT LOGO -->
<br />
<div align="center">
  <a href="https://github.com/dobrasync/client-cli">
    <img src="docs/assets/logo.png" alt="Logo" width="80" height="80">
  </a>

  <h3 align="center">CLI CLient</h3>

  <p align="center">
    Simplest way to connect to a Dobrasync API and sync files.
    <br />
    <a href="https://github.com/dobrasync/client-cli/docs/user"><strong>Explore the docs »</strong></a>
    <br />
    <br />
    ·
    <a href="https://github.com/Dobrasync/cli/issues/new?assignees=&labels=&projects=&template=bug_report.md&title=">Report Bug</a>
    ·
    <a href="https://github.com/Dobrasync/cli/issues/new?assignees=&labels=&projects=&template=feature_request.md&title=">Request Feature</a>
  </p>
</div>


<!-- TABLE OF CONTENTS -->
<details>
  <summary>Table of Contents</summary>
  <ol>
    <li>
      <a href="#getting-started">Getting Started</a>
      <ul>
        <li><a href="#prerequisites">Prerequisites</a></li>
        <li><a href="#installation">Installation</a></li>
      </ul>
    </li>
    <li><a href="#usage">Usage</a></li>
  </ol>
</details>

<!-- GETTING STARTED -->
## Getting Started

### Prerequisites

- .NET Runtime 8 or later ([instructions](https://github.com/Dobrasync/api))
- Linux, Windows 10/11 or MacOS
- Running Dobrasync API ([instructions](https://learn.microsoft.com/en-us/dotnet/core/install/))

### Installation

#### First run

The app will create a `.dobrasync` directory in your OS users home directory. This is where your configuration is stored. You can delete this directory should you wish to completely reset all settings.

#### Setup default library directory

We also need to define where libraries should be downloaded to:

```bash
dobrasync config --library-dir DEFAULT_LIBRARY_DIRECTORY
```

Example library directory: `~/dobrasync-libs`

#### Setup temp block directory

Dobrasync transfers files by chunking them into smaller "blocks" to reduce duplicate data. These blocks need to be temporarily stored somewhere on disk during transfer:

```bash
dobrasync config --temp-block-dir TEMPBLOCK_DIR
```

Example temp block directory: `~/dobrasync-tempblocks`


#### Setup connection to remote

The app needs to know to which remote (Dobrasync API) it should connect to. Do this by running:

```bash
dobrasync config --remote YOUR_REMOTE_URL
```


#### Login

You'll of course need to log in before you can access any data on the remote:

```bash
dobrasync login
```

You will be prompted with a URL to your IdP and a code. Open the displayed URL and enter the code. Click "allow" when prompted for confirmation.

The app will log that sign-in was successful if everything went ok.

#### Verify configuration

You can use `dobrasync config --list` to display the current configuration.


<!-- USAGE EXAMPLES -->
## Usage

Make sure you have properly configured your app before starting to use it.

### Creating a library

```
dobrasync create LIBRARY_NAME
```

The Remote-ID of the newly created library will be displayed after creation. Use this Remote-ID when cloning.  

### Listing libraries

```
dobrasync ls
```

This command lists all libraries connected to your account, remote and local.

### Cloning a library

You clone/download a library by using the clone command:

```bash
dobrasync clone LIBRARY_REMOTE_ID
```

Use `dobrasync ls` to get thee Remote-ID of the library you want to clone.

### Remove a library

> ⚠️ These actions can not be undone! Make sure to read over what you typed before committing.

Use the `remove` command to remove a library.

This will remove the library from local library id, but won't delete any files from local machine.

```bash
dobrasync remove LIBRARY_LOCAL_ID
```

To remove the local clone and its local files:

```bash
dobrasync remove LIBRARY_LOCAL_ID --delete-local
```

To remove the library from local and delete it on remote, but keep files on local machine:

```bash
dobrasync remove LIBRARY_LOCAL_ID --delete-remote
```

To delete the local AND remote library and files on local AND remote: 

```bash
dobrasync remove LIBRARY_LOCAL_ID --delete-local --delete-remote
```

### Sync a specific library

You can manually sync a library by using the `sync` command.

```bash
dobrasync sync LIBRARY_LOCAL_ID
```

### Sync all libraries

Instead of syncing each library individually you can also use the `sync --all` command. This will automatically sync all local libraries.

### Statistics

You can display some statistics by using the `stats` command.

```bash
dobrasync stats
```

### Daemon / Run in Background

TBD

### Autostart

TBD

### Logout

You can sign out of your account by using the `logout` command.

```bash
dobrasync logout
```



<!-- REFLINK -->
[contributors-shield]: https://img.shields.io/github/contributors/dobrasync/client-cli.svg?style=for-the-badge
[contributors-url]: https://github.com/dobrasync/client-cli/graphs/contributors
[forks-shield]: https://img.shields.io/github/forks/dobrasync/client-cli.svg?style=for-the-badge
[forks-url]: https://github.com/dobrasync/client-cli/network/members
[stars-shield]: https://img.shields.io/github/stars/dobrasync/client-cli.svg?style=for-the-badge
[stars-url]: https://github.com/dobrasync/client-cli/stargazers
[issues-shield]: https://img.shields.io/github/issues/dobrasync/client-cli.svg?style=for-the-badge
[issues-url]: https://github.com/dobrasync/client-cli/issues
[license-shield]: https://img.shields.io/github/license/dobrasync/client-cli.svg?style=for-the-badge
[license-url]: https://github.com/dobrasync/client-cli/blob/main/LICENSE.txt
