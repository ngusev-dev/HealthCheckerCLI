<div align="center">
 <img src="https://github.com/user-attachments/assets/43539b07-0ed7-4527-b89f-2a823cd73a88" width="200"  /> 
</div>

<p align="center" style="color: red;">
  <img src="https://img.shields.io/badge/C%23-purple.svg" alt="C#" />
  <img src="https://img.shields.io/badge/License-MIT-white.svg" alt="License MIT">
   <img src="https://img.shields.io/badge/API_MONITORING-red.svg" alt="Api monitoring" />
</p>

<h2 align="center">
  HealthChecker - CLI utility for monitoring services 📱
</h2>

<p align="center">
 <em>Be confident in the working of your services</em>
</p>

### Demo usage

<img width="1129" height="578" alt="image" src="https://github.com/user-attachments/assets/583dcbee-f185-4cd4-9279-58d96e9e119b" />


### 🗃️ File Structure

The services section contains services, where the key is the name of the service that you can use in messageTemplate `{{SERVICE_NAME}}`

```yaml
services:
  api-name:
    url: https://api.dev
    interval: 5
    attempts: 4
  api-name-2:
    url: https://api-2.com/
    interval: 20
    attempts: 3
  api-name-3:
    url: https://api-4.com/
    interval: 10
    attempts: 2
    httpErrorCodes: [401, 500]
notifications:
  tgBotKey: 7545313483:AAEl8JzG_6ro4SDaCOhQGn5vCaeMAtmBvaY
  messageTemplate: "[{{SERVICE_NAME}}] URL: {{SERVICE_LINK}} is unavailable at the moment"
logger:
  logInFile: true
  consoleMode: true
  filePath: './health-checker.txt'
```

## 🗄 Configuration

### 💻 Service Configuration

Each service in the `services` array supports the following parameters:

| Parameter | Required | Description | Example |
|-----------|----------|-------------|---------|
| `url` | ✅ | Service URL to check | `https://api.dev/` |
| `interval` | ❌ | Check interval in seconds. Default - `10` seconds | `30`, `60` |
| `attempts` | ❌ | Number of attempts after which the notification will be sent. Default - `1` | `1`, `4` |
| `httpErrorCodes` | ❌ | A set of status codes that will generate an error.<br> By default, `404, 500, 501, 502, 503, 504, 505, 506, 507, 508, 510, 511` | `[401, 500]` |


### 🔔 Notification Configuration

| Parameter | Required | Description | Example |
|-----------|----------|-------------|---------|
| `tgBotKey` | ❌ | The key to the telegram bot. If it is empty, then notifications are not received through TG | `7545313483:AAEl8JzG_6ro4SDaCOhQGn5vCaeMAtmBvaY` |
| `messageTemplate` | ❌ | Your custom error message.  | Default message: `[{{SERVICE_NAME}}] URL: {{SERVICE_LINK}} is unavailable at the moment` |

> You can use global variables in `messageTemplate` such as:
> 1. `{{SERVICE_NAME}}` - name of the service
> 2. `{{SERVICE_LINK}}` - service url

### ✍️ Logger Configuration

| Parameter | Required | Description | Example |
|-----------|----------|-------------|---------|
| `logInFile` | ❌ | Write logs to a file. Default = `false` | `true` |
| `consoleMode` | ❌ |  Output logs to the console when running the utility. Default = `false` | `true` |
| `filePath` | ❌ | The path to saving logs (it works if `logInFile = true`). Default = `./logs.txt` | `./health-checker.txt` |
