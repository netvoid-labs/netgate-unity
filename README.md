# NetGate SDK for Unity

## Install via UPM (Unity Package Manager)
1. Open Unity
2. Open Package Manager (Window -> Package Manager)
3. Click on the + icon (Add package from git URL...)
4. Enter URL: https://github.com/netvoid-labs/netgate-unity.git#upm

## Dependencies
- NativeWebSocket ( https://github.com/endel/NativeWebSocket )

## Example

```
void Awake()
{
  NetGate.Instance.OnConnected += OnConnected;
}

void Start()
{
  NetGate.Instance.Connect("localhost:4555", "12345678");
}

void OnConnected()
{
  Debug.LogError("Connected");
}
```

## License
MIT
