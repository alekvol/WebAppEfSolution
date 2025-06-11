```mermaid
graph LR
    %% ------- Условные обозначения -------
    classDef user fill:#E6E6FA,stroke:#333,color:#000 %% Лаванда для пользователя
    classDef internet fill:#ADD8E6,stroke:#333,color:#000 %% Голубой для интернета/публичных IP
    classDef vps fill:#FFFACD,stroke:#333,color:#000 %% Лимонный для VPS
    classDef truenas fill:#90EE90,stroke:#333,color:#000 %% Салатовый для TrueNAS
    classDef localnetwork fill:#F0E68C,stroke:#333,color:#000 %% Хаки для локальной сети
    classDef proxy fill:#FFDAB9,stroke:#333,color:#000 %% Персиковый для прокси/туннелей

    %% ------- Блоки -------
    subgraph Посетитель Nextcloud
        UserDevice["Веб-браузер"]:::user
    end

    subgraph Интернет / Публичные IP VPS
        PublicNextcloud["nextcloud.streetmagic.fun:8443"]:::internet
        PublicVLESSforFRP["frp-tunnel.yourdomain.com:4433"]:::internet
        PublicVLESSReality["reality-mask.yourdomain.com:443"]:::internet
    end

    subgraph VPS (Виртуальный Сервер)
        direction LR
        CaddyVPS["Caddy (слушает на 0.0.0.0:8443)"]:::vps
        XRayServer["XRay Сервер (3X-UI)"]:::vps
        FRPS_Server["FRPS (слушает на 127.0.0.1:7000)"]:::vps
    end

    subgraph Локальная Сеть TrueNAS
        direction LR
        subgraph TrueNAS Scale
            FRPC_Client["FRPC"]:::truenas
            XRayClient["XRay Клиент"]:::truenas
            NextcloudApp["Nextcloud Приложение (слушает на IP_Nextcloud:80)"]:::truenas
            NPM_Local["NPM (для локального доступа, не на пути внешнего трафика)"]:::truenas
        end
    end

    %% ------- Соединения и Потоки -------

    %% Поток 1: Пользователь заходит на Nextcloud
    UserDevice -- "1. Запрос HTTPS" --> PublicNextcloud
    PublicNextcloud ===> CaddyVPS
    CaddyVPS -- "2. Проксирует на порт от FRPS" --> FRPS_Server_RemotePort["127.0.0.1:7001 (remote_port)"]:::vps
    FRPS_Server_RemotePort -.-> FRPS_Server

    %% Поток 2: Установление и работа FRP туннеля через VLESS
    FRPC_Client -- "A. Хочет к FRPS (VPS_IP:7000) через SOCKS" --> XRayClient_SOCKS["127.0.0.1:10808 (SOCKS прокси от XRay)"]:::proxy
    XRayClient_SOCKS -.-> XRayClient
    XRayClient -- "B. VLESS туннель" --> PublicVLESSforFRP
    PublicVLESSforFRP ===> XRayServer_VLESS_FRP_In["VLESS Inbound (на VPS:4433)"]:::proxy
    XRayServer_VLESS_FRP_In -.-> XRayServer
    XRayServer -- "C. Перенаправляет на локальный FRPS" --> FRPS_Server
    FRPS_Server <---> FRPC_Client_Via_VLESS["(FRP соединение установлено внутри VLESS)"]:::proxy
    FRPC_Client_Via_VLESS -.-> FRPC_Client

    %% Связь FRPS с remote_port и FRPC с локальным приложением
    FRPS_Server -- "D. Передача данных Nextcloud через туннель" --> FRPC_Client_Via_VLESS
    FRPC_Client -- "E. Проксирует на Nextcloud App" --> NextcloudApp

    %% Поток 3: VLESS+Reality (независимый)
    UserVLESSReality["Пользователь VLESS+Reality"]:::user
    UserVLESSReality -- "VLESS+Reality Запрос" --> PublicVLESSReality
    PublicVLESSReality ===> XRayServer_VLESS_Reality_In["VLESS+Reality Inbound (на VPS:443)"]:::proxy
    XRayServer_VLESS_Reality_In -.-> XRayServer
    XRayServer -- "Выход в интернет" --> InternetIcon["(Интернет)"]

    %% Поток 4: Локальный доступ к Nextcloud через NPM (независимый)
    LocalUserDevice["Локальный ПК"]:::user
    LocalUserDevice -- "Запрос https://nextcloud.local" --> NPM_Local
    NPM_Local -- "Проксирует на Nextcloud App" --> NextcloudApp
    ```
