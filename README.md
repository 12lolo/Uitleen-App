# Uitleen-App (Firda Lending – Mobile Client)

Mobile app for the **Firda Lending System**. Students can browse available equipment; teachers/admins can create and return loans and record damage. The app talks to the Firda Lending API.

> Server-side API: https://github.com/12lolo/uitlenen

---

## ✨ Features

- **Browse & search equipment** by category or name
- **Create/return loans** (teacher/admin accounts)
- **Damage notes** on return
- **Auth & roles** (token-based; student vs teacher/admin)
- **Configurable API base URL** (saved locally per device)
- **Offline-friendly errors** (clear feedback if API is unreachable)

---

## 📱 Platforms

- **Android** (primary target)
- **iOS** (build on macOS; see instructions)

---

## 🧰 Requirements

- **.NET 8 SDK**

- **.NET MAUI workloads**

  ```bash
  dotnet workload install maui
  ```

- **Android**: Android SDK + JDK 17 (installed by MAUI workload)

- **iOS (optional)**: Xcode (on macOS), Apple developer tooling

---

## 🚀 Getting Started

```bash
# Clone
git clone https://github.com/12lolo/Uitleen-App.git
cd Uitleen-App

# Restore
dotnet restore

# Run on Android emulator/device
dotnet build -t:Run -f net8.0-android

# (macOS) Run on iOS Simulator
dotnet build -t:Run -f net8.0-ios
```

> If you haven’t set up Android emulators yet, open **Android Device Manager** (from Visual Studio) and create a device image. On macOS, use Xcode’s **Simulator** for iOS.

---

## ⚙️ Configuration

Set the API base URL once inside the app (settings screen) or via a small config file.

- **Base URL (examples)**  
  - Production: `https://api.example.com`  
  - Local dev (Laravel default): `http://127.0.0.1:8000`

- **Token storage**  
  - Save the access token after login (per user). Prefer secure storage (e.g., `SecureStorage`) where available.

**Sample JSON (if a config file is used):**

```json
{
  "ApiBaseUrl": "http://127.0.0.1:8000",
  "RememberUser": true
}
```

---

## 🔄 Typical Flows

### Create a Loan (teacher/admin)

1. Search/select an item
2. Pick borrower + due date
3. Confirm → `POST /api/loans`

### Return a Loan

1. Find active loan (search or scan, if enabled)
2. Add optional damage notes
3. Confirm return → `POST /api/loans/{id}/return`

*(Keep the client in sync with the current API routes and validation.)*

---

## 🧪 Testing

- Unit/UI tests can live under a `Tests/` project (optional)
- Use a staging API base URL when running automated tests

---

## 📦 Publishing

**Android (APK/AAB):**

```bash
# Release build
dotnet publish -f net8.0-android -c Release

# Signed AAB/APK (configure signing in csproj or via .storepass keystore)
dotnet publish -f net8.0-android -c Release /p:AndroidPackageFormat=aab
```

**iOS (App Store/TestFlight):**

- Requires macOS + Apple developer account
- Use `dotnet publish -f net8.0-ios -c Release` then Xcode/Transporter for upload

---

## 🐛 Troubleshooting

- **401/403**: Check token + role on the API
- **Network/CORS**: Ensure API URL is correct and reachable from device
- **Time sync**: Large clock drift may cause token issues

---

## 🗺 Roadmap (suggested)

- Barcode/QR scan for quick loan lookup
- Bulk return workflow
- Admin dashboard views (due soon / overdue)

---

## 📄 License

For educational use at Firda (choose or adjust a license in the repo).

---

## ✍️ Author

**Senne Visser**  
GitHub: [12lolo](https://github.com/12lolo)
