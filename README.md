# Frends.Community.PaymentServices.Nordea
FRENDS task for using payments services of Nordea bank. With the task it's possible to fetch filelists, download files, upload files and delete files.

- [Installing](#installing)
- [Tasks](#tasks)
  - [Get User Info](#getuserinfo)
  - [Download File List](#downloadfilelist)
  - [Upload File](#uploadfile)
  - [Download File](#downloadfile)
- [License](#license)
- [Building](#building)
- [Contributing](#contributing)
- [Change Log](#change-log)

# Installing
You can install the task via FRENDS UI Task View or you can find the nuget package from the following nuget feed
'Nuget feed coming at later date'

Tasks
=====

## GetUserInfo

Downloads agreement specific information on the agreed Cash Management services

### Task Parameters

| Property             | Type                 | Description                          | Example |
| ---------------------| ---------------------| ------------------------------------ | ----- |
| Url | string | URL of the bank's web service | http://filetransfer.test.nordea.com/services/CorporateFileService |
| Certificate Issued By | string | The issuer of the certificate that should be used for signing the messages |  |
| Environment | string | Target environment (TEST or PRODUCTION) | TEST |
| Customer Id | string | Unique number identifying the bank's customer | 0000000000 |
| Target Id | string | The logical folder name where the file(s) of the customer are stored in the bank. |  |
| Request Id | int | A unique integer value identifying the request | 1 |
| Connection timeout seconds | int | Timeout in seconds to be used for the connection and operation. | 30 |


### Result

| Property             | Type                 | Description                          | Example |
| ---------------------| ---------------------| ------------------------------------ | ----- |
|                      | JToken array         |  |  |

## DownloadFileList

Downloads a list of available files in Nordea Bank's system

### Task Parameters


| Property             | Type                 | Description                          | Example |
| ---------------------| ---------------------| ------------------------------------ | ----- |
| Url | string | URL of the bank's web service | http://filetransfer.test.nordea.com/services/CorporateFileService |
| Certificate Issued By | string | The issuer of the certificate that should be used for signing the messages |  |
| Environment | string | Target environment (TEST or PRODUCTION) | TEST |
| Customer Id | string | Unique number identifying the bank's customer | 0000000000 |
| Request Id | int | A unique integer value identifying the request | 1 |
| File Type | string | Optional parameter for filtering filelists | pain.001.001.02 |
| Target Id | string | The logical folder name where the file(s) of the customer are stored in the bank. |  |
| Start Date| string | Optional parameter for filtering filelists. Files created after will be returned. If this value is null, or unparseable to a DateTime object, no filter is applied. | 2018-05-25 |
| End Date | string |  Optional parameter for filtering filelists. Files created before will be returned. If this value is null, or unparseable to a DateTime object, no filter is applied.| 2018-05-28 |
| Status | string |  Optional parameter for filtering filelist. Valid values for files sent to the bank by the customer are "WFP" or "FWD" (WFP = waiting for processing. FWD = forwarded). Valid values for files sent to the bank by the customer are "NEW" or "DLD" (NEW = files not downloaded yet. DLD = files already downloaded). If no parameter is given or if the status is "ALL", all files will be listed. | NEW |
| Connection timeout seconds | int | Timeout in seconds to be used for the connection and operation. | 30 |

### Result

| Property             | Type                 | Description                          | Example |
| ---------------------| ---------------------| ------------------------------------ | ----- |
|                      | JToken array | Array elements have the following properties: FileReference, TargetId, ParentFileReference, FileType, FileTimestamp, Status |  |

## UploadFile

Uploads a file to Nordea Bank's system


### Task parameters


| Property             | Type                 | Description                          | Example |
| ---------------------| ---------------------| ------------------------------------ | ----- |
| Url | string | URL of the bank's web service | http://filetransfer.test.nordea.com/services/CorporateFileService |
| Certificate Issued By | string | The issuer of the certificate that should be used for signing the messages |  |
| Environment | string | Target environment (TEST or PRODUCTION) | TEST |
| Customer Id | string | Unique number identifying the bank's customer | 0000000000 |
| Request Id | int | A unique integer value identifying the request | 1 |
| File Input | string | File input (e.g. XML content of file) |  |
| File Type | string | File type to upload | pain.001.001.02 |
| File Encoding | string | File encoding for the input file | utf-8 |
| Target Id | string | The logical folder name where the file(s) of the customer are stored in the bank. |  |
| Connection timeout seconds | int | Timeout in seconds to be used for the connection and operation. | 30 |

### Result

| Property             | Type                 | Description                          | Example |
| ---------------------| ---------------------| ------------------------------------ | ----- |
|                      | JToken array | Array elements have the following properties: FileReference, TargetId, ParentFileReference, FileType, FileTimestamp, Status |  |

## DownloadFile

Downloads a list of available files in Nordea Bank's system

### Task Parameters


| Property             | Type                 | Description                          | Example |
| ---------------------| ---------------------| ------------------------------------ | ----- |
| Url | string | URL of the bank's web service | http://filetransfer.test.nordea.com/services/CorporateFileService |
| Certificate Issued By | string | The issuer of the certificate that should be used for signing the messages |  |
| Environment | string | Target environment (TEST or PRODUCTION) | TEST |
| Customer Id | string | Unique number identifying the bank's customer | 0000000000 |
| Request Id | int | A unique integer value identifying the request | 1 |
| File Reference | string | File reference id for the file to be downloaded | 123456 |
| File Encoding | string | File encoding for the input file | utf-8 |
| File Type | bool | Optional parameter for filtering filelists | pain.001.001.02 |
| Target Id | string | The logical folder name where the file(s) of the customer are stored in the bank. |  |
| Status | string |  Optional parameter for filtering filelist. Valid values for files sent to the bank by the customer are "WFP" or "FWD" (WFP = waiting for processing. FWD = forwarded). Valid values for files sent to the bank by the customer are "NEW" or "DLD" (NEW = files not downloaded yet. DLD = files already downloaded). If no parameter is given or if the status is "ALL", all files will be listed. | NEW |
| Connection timeout seconds | int | Timeout in seconds to be used for the connection and operation. | 30 |


### Result

| Property             | Type                 | Description                          | Example |
| ---------------------| ---------------------| ------------------------------------ | ----- |
|                      | string | File content in string format |  |

# License

This project is licensed under the MIT License - see the LICENSE file for details

# Building

Clone a copy of the repo

`git clone https://github.com/CommunityHiQ/Frends.Community.PaymentServices.Nordea.git`

Restore dependencies

`nuget restore frends.community.email`

Rebuild the project

The code cannot be properly tested without connection to the Web Service, so no separate tests are included.

Create a nuget package

`nuget pack nuspec/Frends.Community.Email.nuspec`

# Contributing
When contributing to this repository, please first discuss the change you wish to make via issue, email, or any other method with the owners of this repository before making a change.

1. Fork the repo on GitHub
2. Clone the project to your own machine
3. Commit changes to your own branch
4. Push your work back up to your fork
5. Submit a Pull request so that we can review your changes

NOTE: Be sure to merge the latest from "upstream" before making a pull request!

# Change Log

| Version             | Changes                 |
| ---------------------| ---------------------|
| 1.0.0 | Initial version of Nordea WS tasks |
| 1.0.1 | Small fixes to namespaces |
| 1.0.2 | Fix to download file task |
| 1.0.4 | Small fixes to response namespaces |
| 1.0.5 | Intermediate version for debugging |
| 1.0.6 | Fix to response signature validation |
| 1.0.7 | Updated UploadFile result format |
| 1.0.8 | Bug fix to UploadFile result format |
