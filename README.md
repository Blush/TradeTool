# TradeTool

## Overview

**TradeTool** is an experimental console utility designed to calculate the market price for a given instrument and quantity. Developed in 2024 by Dmitrii Gavrilov.

## Author

Dmitrii Gavrilov  
Email: [blush.post@gmail.com](mailto:blush.post@gmail.com)

## Purpose

The primary purpose of TradeTool is experimental. It is designed to fetch market data from Binance and calculate the average execution price of a market order for a specified instrument and quantity.

## Features

### TradeTool has a single primary function:
Calculate the market price for a given instrument and quantity based on data retrieved from Binance.

### Additional characteristics include:
Fetching the order book from Binance.
Supporting both buy and sell orders.
An extensible architecture designed to support additional exchanges in the future.

## Usage

```shell
TradeTool <instrument> <quantity> [-v|-verbose] [-h|-help]
```

### Parameters

- `instrument`: A string representing the trading pair (e.g., BTCUSDT). The maximum length is 10 characters.
- `quantity`: A decimal number representing the quantity. Enter a positive number for a buy order and a negative number for a sell order.
- `-v | -verbose`: (Optional) Display verbose information.
- `-h | -help`: (Optional) Display this help message.

## Example

```shell
TradeTool BTCUSDT 1.5
```

This command calculates the average execution price for buying 1.5 BTCUSDT on Binance.

## Initial Task

1. Develop a utility to calculate the execution price of a market order on Binance.
2. The utility should be a command-line tool that accepts the ticker name (`Ticker`) and quantity (`Qty`) as arguments.
3. Positive `Qty` indicates a buy order, while negative `Qty` indicates a sell order.
4. Fetch the order book from Binance and calculate the average execution price for the specified quantity.
5. The architecture should be extensible to allow for the addition of other exchanges in the future.

## License

TradeTool - experimental application. (C)Dmitrii Gavrilov 2024 blush@gmail.com

---

For further information or queries, please contact Dmitrii Gavrilov at [blush.post@gmail.com](mailto:blush.post@gmail.com).

---
