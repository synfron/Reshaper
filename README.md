# Reshaper
Traffic debugger for text based protocols over TCP/IP

![ReshaperScreenshot](https://user-images.githubusercontent.com/48854453/54864904-00e29c00-4d34-11e9-8120-a9b6df6f6eea.png)

## Features
- View, manipulate, and trigger actions on incoming and outgoing TCP/IP proxy traffic using configurable rules
- JavaScript engine integration
- Extensible using the Managed Extensibility Framework

## How Does It Work
Reshaper functions as a configurable proxy. Basic proxy configrations are created in the Settings and allow users to specify the TCP port the proxy will listen on, and the target server to direct traffic to. Everything the proxy does is based on rules found in the Text Rules tab (for Text proxy configurations) or HTTP Rules tab (for HTTP proxy configurations). By default, rules are provided in Reshaper that allow for basic data pass through and display once a connection to Reshaper is established. However, even these default rules can be disabled or changed, and new rules can be added. Rules can be created to connect, disconnect, auto-respond, run scripts, modify data, ignore data, transfer data, and display data based on constraints the user specifies.

## Basic Usage Example
Most text protocols follow the same setup as IRC and FTP below. There may be variations in the message delimiter the protocol usas. If the protocol does not have a simple character based message delimiter, uncheck _Use Delimiter_.
_Note: Reshaper does not support TLS connections at this time._

### IRC
1. Go to the Settings tab
2. Click Proxies
3. Click New...
4. Proxy Name: <Any name>
5. Port: <A port Reshaper will listen on, e.g. 6667>
6. Proxy Type: Text
7. Destination Host: <Any IRC server hostname, e.g. irc.rizon.net>
8. Destination Port: <IRC server's port, e.g. 6667>
9. Use Delimiter: Check
10. Add Delimiter: \r\n
11. Add Delimiter: \n
12. Enabled: Check
13. Auto-Activate: <Check: To activate the proxy whenever Reshaper starts>
14. Click Save
15. Connect your IRC client to 127.0.0.1 at the Port specified at Step 5
16. Click the Text tab to view incoming and outgoing traffic

### FTP
1. Go to the Settings tab
2. Click Proxies
3. Click New...
4. Proxy Name: <Any name>
5. Port: <A port Reshaper will listen on, e.g. 21>
6. Proxy Type: Text
7. Destination Host: <Any FTP server hostname, e.g. coast.cs.purdue.edu>
8. Destination Port: <FTP server's port, e.g. 21>
9. Use Delimiter: Check
10. Add Delimiter: \r\n
11. Add Delimiter: \n
12. Enabled: Check
13. Auto-Activate: <Check: To activate the proxy whenever Reshaper starts>
14. Click Save
15. Connect your FTP client to 127.0.0.1 at the Port specified at Step 5
16. Click the Text tab to view incoming and outgoing traffic
  
### HTTP
_Note: HTTP support is limited and may cause websites to display improperly. HTTPS is not yet supported._ 
1. Click View > HTTP Event in the Menu Bar
2. Go to the Settings tab
3. Click Proxies
4. Click New...
5. Proxy Name: <Any name>
6. Port: <A port Reshaper will listen on, e.g. 8888>
7. Proxy Type: Http
8. Enabled: Check
9. Register As System Proxy
10. Auto-Activate: <Check: To activate the proxy whenever Reshaper starts>
    - Your browser must be configured to use the System proxy. If the Reshaper proxy must be manually set in the browser.
11. Navigate your browser to a http:// website.
11. Click the HTTP tab in Reshaper to view captured traffic.

## To Do
- Improve native HTTP support
- Improve UI visuals and usability
- Add usage and dev documentation

## Contributions
Contributions are encouraged, especially those that address the To Do items. Issues and Pull Requests welcome. Also help us spread the word.

## License
MIT License. See [LICENSE](https://raw.githubusercontent.com/synfron/Reshaper/master/LICENSE)
