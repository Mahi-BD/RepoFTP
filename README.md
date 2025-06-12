# RepoFTP - FTP Folder Synchronizer

A Windows Forms application built with VB.NET that provides FTP synchronization and folder cleaning capabilities.

## Features

### 🔄 FTP Synchronization
- **Bidirectional sync**: Local to Remote or Remote to Local
- **Automatic connection handling** with multiple transfer modes (AutoPassive, AutoActive, PORT)
- **Recursive directory sync** maintains folder structure
- **Progress tracking** with real-time updates
- **Error resilience** continues operation even if some files fail

### 🧹 Folder Cleaning
- **Configurable folder cleaning** via Settings menu
- **Smart detection** only cleans folders that exist
- **Force delete option** removes read-only attributes
- **Safe operation** deletes contents but preserves folder structure
- **Confirmation dialog** shows exactly what will be cleaned

### 📊 Logging & Monitoring
- **Comprehensive logging** with multiple levels (Debug, Info, Warning, Error)
- **Log viewer** with syntax highlighting
- **FTP directory browser** to explore remote server structure
- **Connection testing** with detailed feedback

### ⚙️ Configuration
- **Persistent settings** stored in `config.ini`
- **Encrypted password storage** for security
- **Auto-save configuration** with debounced updates
- **Clean folder customization** via Settings dialog

## Installation

1. Download the latest release
2. Extract to your desired folder
3. Run `RepoFTP.exe`

### Requirements
- Windows 10/11
- .NET 8.0 Runtime
- Network access for FTP operations

## Usage

### Initial Setup
1. **Configure FTP Connection**:
   - Enter FTP server address (e.g., `192.168.1.100` or `ftp.example.com:2121`)
   - Provide username and password
   - Test connection using the "🔌 Test Connection" button

2. **Select Local Folder**:
   - Click "Browse" to select your local directory
   - This folder will be used for sync operations

3. **Configure Remote Path** (optional):
   - Enter remote folder path (e.g., `/uploads/` or leave empty for root)

### Synchronization

#### Local to Remote
- Select "Local to Remote" option
- Click "🔄 Sync" to upload files from local to FTP server
- All files and subdirectories will be uploaded recursively

#### Remote to Local
- Select "Remote to Local" option
- Click "🔄 Sync" to download files from FTP server
- Maintains directory structure locally

### Folder Cleaning

#### Configure Clean Settings
1. Go to **File → Settings**
2. Configure folders to clean (comma-separated): `bin,obj,packages,.vs,Debug,Release`
3. Enable/disable force delete for read-only files
4. Click "Save"

#### Clean Operation
1. Select a local folder for sync
2. Click "🧹 Clean" button
3. Review confirmation dialog showing which folders will be cleaned
4. Confirm to proceed with cleaning

## Configuration File

Settings are stored in `config.ini` in the application directory:

```ini
# FTP Configuration (encrypted)
[FTP]
Server=ftp.example.com
Username=myuser
Password=encrypted_password_here

# Sync Configuration
[Sync]
LocalFolder=C:\MyProject
RemoteFolder=/uploads/
Direction=LocalToRemote

# Clean Configuration
CleanFolders=bin,obj,packages,.vs,Debug,Release
ForceDelete=True
```

## Supported FTP Servers

- **Standard FTP** (Port 21)
- **Custom ports** (e.g., `server:2121`)
- **Passive and Active modes** with automatic fallback
- **Various server types** (Windows IIS, Linux vsftpd, etc.)

## Server URL Formats

```
192.168.1.100              # IP with default port 21
ftp.example.com             # Domain with default port 21
192.168.1.100:2121          # IP with custom port
ftp.example.com:2121        # Domain with custom port
ftp://192.168.1.100:2121    # Full FTP URL
```

## Clean Folder Examples

### Development Projects
```
bin,obj,packages,node_modules,.vs,Debug,Release
```

### Build Artifacts
```
dist,build,out,target,.gradle,CMakeFiles
```

### Cache Folders
```
.cache,.tmp,temp,logs,.log
```

## Logging

Log files are automatically managed:
- **Location**: Application directory
- **Retention**: 7 days (configurable)
- **Levels**: Debug, Info, Warning, Error, Critical
- **View logs**: Click "📄 Logs" button

## Error Handling

### FTP Connection Issues
- **Automatic retry** with different connection modes
- **Detailed error logging** for troubleshooting
- **Connection timeout** protection (30 seconds)
- **Network resilience** handles temporary disconnections

### File Operation Errors
- **Continues on errors** doesn't stop entire operation
- **Detailed error reporting** shows which files failed
- **Force delete option** handles read-only files
- **Permission handling** graceful degradation

## Security Features

- **Encrypted password storage** using Windows DPAPI
- **No plaintext passwords** in configuration files
- **Secure connection handling** with certificate validation options
- **Local-only storage** credentials never transmitted except to FTP server

## Troubleshooting

### Connection Problems
1. **Test Connection** - Use the test button to verify FTP settings
2. **Check Firewall** - Ensure FTP ports are not blocked
3. **Try Different Modes** - Application automatically tries multiple connection modes
4. **Verify Credentials** - Double-check username and password

### Sync Issues
1. **Check Permissions** - Ensure read/write access to local folder
2. **Verify Remote Path** - Use FTP browser to confirm remote directory exists
3. **Review Logs** - Check detailed logs for specific error messages
4. **Free Space** - Ensure adequate disk space for downloads

### Clean Operation Issues
1. **Close Applications** - Ensure no programs are using files in target folders
2. **Enable Force Delete** - Use force delete option for read-only files
3. **Run as Administrator** - May be required for system-protected files
4. **Check Logs** - Review detailed error information

## Building from Source

### Prerequisites
- Visual Studio 2022 or later
- .NET 8.0 SDK
- Windows SDK

### Build Steps
```bash
git clone <repository-url>
cd RepoFTP
dotnet restore
dotnet build --configuration Release
```

### Dependencies
- **FluentFTP 52.1.0** - FTP client library
- **.NET 8.0 Windows Forms** - UI framework

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests if applicable
5. Submit a pull request

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Support

For issues and questions:
1. Check the troubleshooting section above
2. Review log files for detailed error information
3. Create an issue with:
   - Application version
   - Operating system
   - FTP server type
   - Error logs (remove sensitive information)

## Version History

### v2.0
- Added folder cleaning functionality
- Implemented config.ini support
- Enhanced error handling and logging
- Improved UI with better button layout
- Added Settings dialog for clean configuration

### v1.0
- Initial release
- Basic FTP synchronization
- Encrypted configuration storage
- Log viewer and FTP browser