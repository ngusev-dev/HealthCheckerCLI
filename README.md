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

### 🗃️ File Structure

The services section contains services, where the key is the name of the service that you can use in messageTemplate `{{SERVICE_NAME}}`

```yaml
services:
  api-name:
    link: https://api.dev
    interval: 5
    attempts: 4
  api-name-2:
    link: https://api-2.com/
    interval: 20
    attempts: 3
  api-name-3:
    link: https://api-4.com/
    interval: 10
    attempts: 2
    httpErrorCodes: [401, 500]
notifications:
  tgBotKey: 7545313483:AAEl8JzG_6ro4SDaCOhQGn5vCaeMAtmBvaY
  messageTemplate: "[{{SERVICE_NAME}}] URL: {{SERVICE_LINK}} не доступен в данный момент"
```

## 🗄 Configuration

### Service Configuration

Each service in the `services` array supports the following parameters:

| Parameter | Required | Description | Example |
|-----------|----------|-------------|---------|
| `link` | ✅ | Service URL to check | `https://api.dev/` |
| `interval` | ✅ | Check interval in milliseconds | `30`, `60` |
| `httpErrorCodes` | ❌ | A set of status codes that will generate an error.<br> By default, `404, 500, 501, 502, 503, 504, 505, 506, 507, 508, 510, 511` | `[401, 500]` |
