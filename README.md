# How To Install

Add the lines below to Packages/manifest.json

- for version 2.146.221018
```csharp
"com.pancake.gameservice": "https://github.com/pancake-llc/game-service.git?path=Assets/_Root#2.146.221018",
"com.lupidan.apple-signin-unity": "https://github.com/lupidan/apple-signin-unity.git#v1.4.2",
"com.coffee.ui-effect": "https://github.com/mob-sakai/UIEffect.git#4.0.0-preview.9",
"com.coffee.ui-particle": "https://github.com/mob-sakai/ParticleEffectForUGUI.git#4.1.6",
```

Dependency : Facebook

````csharp
"com.pancake.facebook": "https://github.com/pancake-llc/facebook.git?path=Assets/_Root#1.0.7",
"com.google.external-dependency-manager": "https://github.com/pancake-llc/external-dependency-manager.git?path=Assets/_Root#1.2.170",
````

Dependency : Heart
```csharp
"com.pancake.heart": "https://github.com/pancake-llc/heart.git?path=Assets/_Root",
```


![playfab-anonymous-login-and-recoverable-login-min](https://user-images.githubusercontent.com/44673303/166100604-75c5949d-8c71-4b67-abbc-eb752ec51bfa.png)

![compact_login](https://user-images.githubusercontent.com/44673303/166114223-13fb92e7-00cc-4947-b33f-50f54acf2270.png)


# Usages

- cài đặt setting thông qua phím tắt `alt + 4`
- ![image](https://user-images.githubusercontent.com/44673303/193963879-16e7337d-3ebe-42b2-a700-feff49f1f1b0.png)
- ![image](https://user-images.githubusercontent.com/44673303/193964093-d1d78788-3fe8-49ca-9036-1b063e65ac59.png)
- sử dụng menu `Update Aggregation` để tạo table leaderboard cho 240 country chỉ cần thực hiện điều này 1 lần
- cài đặt package demo-imnplemention trong release page
- thay thế các đoạn code nằm trong `#if region replace your code` bằng code của bạn
- for update score to leaderboard when first time you enter name complete. You can via using `valueExpression` in `ButtonLeaderBoard`

```c#
GetComponent<ButtonLeaderboard>().valueExpression += () => UnityEngine.Random.Range(1, 100);
```