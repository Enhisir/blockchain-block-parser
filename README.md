# Simple Bitcoin Block Parser

Простой парсер блока биткоина.

`BlockchainBlockBuilder` считывает данные из байтового потока и строит структуру блока.

`BlockchainBlockExporterCsv` экспортирует данные в формате `.csv`.

Данные сохраняются в двух файлах:
* block-<хэш блока>-info.csv содержит данные заголовка блока, количество транзакций и размер блока;
* block-<хэш блока>-transactions.csv представляет собой список транзакций.


*** Подробнее про структуру блока Blockchain и Bitcoin
* https://learnmeabitcoin.com/ [ENG] - Блог о BTC и его техническом устройстве с документацией, а также примерами.
* https://habr.com/ru/articles/319868/ - цикл статей на Хабре