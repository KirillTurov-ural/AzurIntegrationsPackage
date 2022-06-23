### Azur Integrations Package
Данный пак является примером и полностью функциональной интеграцией основных библиотек, необходимых
для корректной работы проектов, публикуемых посредством Azur Games

### Список интеграций
AppLovin - APPLOVIN_INT
AppsFlyer - APPSFLYER_INT
AppMetrica - APPMETRICA_INT
Facebook - FACEBOOK_INT
Firebase - FIREBASE_INT

### Установка
Первый способ - При помощи Unity Package Manager
1. В этом случае производ, открываем Window->Package Manager
2. Добавляем новый пак -> Git URL
3. Вставляем в строку адрес репозитория https://github.com/KirillTurov-ural/AzurIntegrationsPackage.git

Второй способ - при помощи саб репозитория (если у вас проект на гите)
1. Открываем ваш репозиторий
2. Добавляем саб репозиторий https://github.com/KirillTurov-ural/AzurIntegrationsPackage.git
3. Пример интеграции с саб репозиторием -https://github.com/KirillTurov-ural/AzurIntegrations.git

Третий способ - при помощи Unity Package - нужно скачать только один файл, который содержит последнюю версию интеграций

Общие действия, вне зависимости от типа интеграции
1. Добавляем все необходимые сторонние библиотеки. 
2. В завимости от необходимых библиотек, заходим в Player Settings и прописываем директивы, которые соответствуют интегрированным библиотекам. Все директивы прописаны выше, около каждой из интеграций. Только после этого начнёт работать код, связанный с конкретной интеграцией. 
3. Для работы AppMetrica также необходимо создать Assembly Definition. Для этого, в папке интегрированной библиотеки щёлкаем правой кнопкой мышки и создаём Assembly Definition с названием AppMetrica, также заходим в папку AppMetrica/Editor и создаём Assembly Definition с название AppMetrica_Editor. После этого, плагин получит доступ к внешней библиотеке. 
4. Пример работы интеграции через Unity Package Manager по ссылке - https://github.com/KirillTurov-ural/AzurIntegrationSamplePackageManager.git


### Что есть в данном паке
1. Примеры работы с аналитикой и рекламой
2. Примеры отправки базовых событий аналитике (из базового ТЗ по гиперкэжу) - video_ads_watch, started, available, level_start, level_finish
3. Примеры показа рекламы - interstitial, rewarded, banner
4. Удаление данных для iOS с полным запретом отправки аналитики - новое требование от платформы. 

### Примечание
Мы постоянно дополняем и улучшаем данный пак, по всем предложениям можете обращаться к Kirill Turov напрямую. 

### Code example:
```csharp

```

