# 🪟 Win10 Gadgets – WebView2 Widgets

Egy kis gyűjtemény Windows-os “widget” jellegű alkalmazásokból, melyek webes szolgáltatásokat csomagolnak natív WPF alkalmazásba **Microsoft Edge (WebView2) motorral**.

A projekt elsődleges célja egy **Messenger desktop pótlás**, mivel a hivatalos Messenger Windows app megszűnt, és hamarosan a `messenger.com` weboldal is kivezetésre kerül.

---

## Fő ötlet

- Natív Windows alkalmazás  
- Edge-alapú WebView2 motor  
- Saját ablak, saját ikon, tray integráció  
- Ugyanaz az élmény, mint egy “igazi” desktop app  

Technikailag a webes felületet használja, de **nem egy böngészőablak**, hanem egy dedikált, könnyű WPF alkalmazás.

---

## Jelenlegi widgetek

### 💬 MessengerWidget
Messenger desktop pótlás:

- Facebook Messenger webes felület használata  
- Kamera és mikrofon engedélyezve (videóhívás működik)  
- Új ablakok ugyanabban az ablakban nyílnak meg  
- Tray ikon (Show / Hide / Exit)  
- X gomb = elrejtés, nem bezárás  

Alapértelmezett induló oldal:  
https://www.facebook.com/messages  

Fallback támogatás:  
https://www.messenger.com  

---

## Technológiák

- .NET 8 (WPF)
- Microsoft WebView2
- Hardcodet.NotifyIcon.Wpf (tray ikon)

---

## Projekt struktúra

win10gadgets_projekt/
│
├─ win10gadgets_projekt.sln
├─ MessengerWidget/
│ └─ MessengerWidget.csproj
├─ HboMaxWidget/ (később)
└─ Shared/ (később)

---

Monorepo felépítés: egy repo, több widget projekt.

---

## Build

```
dotnet build win10gadgets_projekt.sln
```

## Futtatás
```
dotnet run --project .\MessengerWidget\MessengerWidget.csproj
```
---

Publish (Release EXE)
```
dotnet publish .\MessengerWidget\MessengerWidget.csproj `
  -c Release `
  -r win-x64 `
  --self-contained false
```

## Az elkészült futtatható fájl:
```
MessengerWidget\bin\Release\net8.0-windows\win-x64\publish\MessengerWidget.exe
```
---

## Használat:

- Indítás után megjelenik az ablak
- Tray ikon bal klikk: show / hide
- Tray ikon jobb klikk: menü
- X gomb: elrejtés
- Exit: teljes kilépés

---

## Miért nem böngésző?

- Ez nem egy “becsomagolt Edge”, hanem egy dedikált alkalmazás, ami:
- gyorsabban indul
- kevesebb UI elemet tartalmaz
- nem keveredik a normál böngésző munkamenettel
- különálló profil mappát használ

---

## Jövőbeli tervek

- HBO Max widget
- Netflix widget
- ...
- Installer (Inno Setup)
