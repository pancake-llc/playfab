# What
PlayFab SDK for Unity

Add the lines below to Packages/manifest.json

for version `2.153.221024`
```csharp
"com.pancake.playfab": "https://github.com/pancake-llc/playfab.git?path=Assets/_Root#2.153.221024",
```

![playfab-anonymous-login-and-recoverable-login-min](https://user-images.githubusercontent.com/44673303/166100604-75c5949d-8c71-4b67-abbc-eb752ec51bfa.png)

![compact_login](https://user-images.githubusercontent.com/44673303/166114223-13fb92e7-00cc-4947-b33f-50f54acf2270.png)


# Usages

- cài đặt setting thông qua menu `Tool/Pancake/Playfab`
- ![image](https://user-images.githubusercontent.com/44673303/193963879-16e7337d-3ebe-42b2-a700-feff49f1f1b0.png)
- ![image](https://user-images.githubusercontent.com/44673303/193964093-d1d78788-3fe8-49ca-9036-1b063e65ac59.png)
- sử dụng menu `Update Aggregation` để tạo table leaderboard cho 240 country chỉ cần thực hiện điều này 1 lần
- cài đặt package demo-imnplemention trong release page
- thay thế các đoạn code nằm trong `#if region replace your code` bằng code của bạn
- for update score to leaderboard when first time you enter name complete. You can via using `valueExpression` in `ButtonLeaderBoard`

```c#
GetComponent<ButtonLeaderboard>().valueExpression += () => UnityEngine.Random.Range(1, 100);
```