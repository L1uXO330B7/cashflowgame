// Game Phases
//   Phase 1 - Dream Phase
//      Choose your path and destination (Occupation & Dream)
//		Each dream will have an interest perk that can earn and save money at times in the game
//   Phase 2 - Rat Race
//      Starting out - 0 assets, pay off liabilities, buy low sell high
//      Middle/the race - 1st big acquisition
//      End game - paid off liabilities and looking for a big deal
//   Phase 3 - Fast Track (Acquire dream)
//      Cash flow day - start with 100 times rat race passive income
//      New income = cashflow day income + 50k
//      New rules - roll 2 die * cannot borrow money from bank

// https://stackoverflow.com/questions/16284724/what-does-var-app-app-do
// 避免重複名稱蓋到別人的 APP
var APP = APP || {
    //應該是拿來存檔用的Obj?
    players: [],
    pCount: 1,
    turnCount: 1,
    currentPlayer: 1,
    saveKey: '',
    // 目前的玩家
    currentPlayerArrPos: function () {
        return APP.currentPlayer - 1;
    },
    // 上一位玩家
    previousPlayer: function () {
        var prevPlayer;

        if (this.currentPlayer === 1) {
            prevPlayer = this.pCount; //應該是利用玩家人數控制回合或控制當回合是誰的回合?
        } else {
            prevPlayer = APP.currentPlayerArrPos();
        }

        return prevPlayer;
    },
    // 初始選擇幾位玩家的部份的 Id 他是寫死的不知原因，藉由丟進來的數字可以在 143 行再丟到設定檔內
    name: function (playerNumber) {
        var player = "player" + parseInt(playerNumber, 10) + "name";//html id 為 player[i]name
        var name = document.getElementById(player).value; //再抓輸入的value
        return name;
    },
    // 判斷是因為這專案起的蠻早的 2018 此處是用 jq 去監聽事件，對我寫框架經驗大於較原生地寫法來說有點不習慣看他這樣寫
    initGame: function () {
        // 起始按鈕
        $("#home-screen").click(function () {
            // 此處有分了一個 display 模組在 display.js 主要是用來控制 CSS
            APP.display.hideHomeScreen();
            APP.display.showGameSelectionScreen();
            window.setTimeout(function () {
                if ($("#game-selection-screen").css('display') != 'none' && $("#turn-info").css('display') != 'inline-block') {
                    window.location.reload(false);
                }
            }, 120000);
        });
        // 新遊戲按鈕 (感覺有點多餘)
        $("#new-room-button").click(function () {
            APP.display.hideGameSelectionScreen();
            APP.display.showGameSetupScreen();
        });
        // 因為是單機設定完後要開始遊戲
        $("#start-game").click(function () {
            $("#window").css("background-image", "");
            $("#window").css("background-color", "white" /*"#4E342E"/*#010410*/);
            APP.display.hideSetup();
            APP.display.renderBoard();
        });
        // 繼續遊戲
        $("#continue-game").click(function () {
            $("#window").css("background-image", "");
            $("#window").css("background-color", "white" /*"#4E342E"/*#010410*/);
            APP.display.hideSetup();
            APP.display.showFinanceBox();
            APP.display.renderBoard();
        });

        //APP.saveKey = Math.random().toString(36).substring(7);
    },
    setup: function (gameState) {
        // 新遊戲靠 html click 丟參數近來判別
        if (gameState == 'new game') {
            // Create players
            var pn = document.getElementById("player-number");

            // 原生 js 取下拉索引，此處 value 是單純的數字 (幾人玩)
            APP.pCount = pn.options[pn.selectedIndex].value;
            // 不確定與上一句差別，此處兩者都相同的可看 log
            APP.remainingPlayers = this.pCount;
            console.log('pCount === this.pCount', APP.pCount, this.pCount, APP.pCount === this.pCount);

            // Get colors for each player
            var newColorsArr = [];

            // 這裡感覺寫得太複雜，單純就是取玩家選完的顏色，可能是以前沒有框架的雙向綁定
            for (var a = 1; a <= APP.pCount; a++) {
                var colorId = document.getElementById(("color-input-player" + parseInt(a, 10)));

                if (colorId.options[colorId.selectedIndex].value !== 'Random Color') {
                    newColorsArr.push(colorId.options[colorId.selectedIndex].value);
                }
            }

            // 取得 html 選擇的選項，去依玩家順序設定初始值
            for (var i = 1; i <= APP.pCount; i++) {
                // Choose selected scenario
                var jobId = "job-input-player" + parseInt(i, 10);
                var pj = document.getElementById(jobId);
                var colorId = "color-input-player" + parseInt(i, 10);
                var pc = document.getElementById(colorId);

                // 這裡是動態渲染出來的
                var tableRow = "table-row-player" + parseInt(i, 10);
                var rowId = document.getElementById(tableRow);

                // 隨機工作
                // Get selected job
                if (pj.options[pj.selectedIndex].value === 'Random Job') {
                    var playerScenario = Math.floor(
                        // Excludes ceo job from random job
                        Math.random() * (APP.scenarioChoices.length - 1)
                    );
                } else {
                    var playerScenario = pj.selectedIndex - 1;
                }

                // 隨機顏色
                // Set selected color
                var playerColor;

                if (pc.options[pc.selectedIndex].value === 'Random Color') {
                    function getRandomColor() {
                        return Math.floor(Math.random() * (APP.display.playerColors.length));
                    }

                    var randVal = getRandomColor();


                    /*newColorsArr.forEach((ele)=>{
                        playerColor = pc.options[randVal].value;

                        if (ele !== pc.options[randVal].value){

                            newColorsArr.push(playerColor)
                        }
                    })*/

                    playerColor = pc.options[randVal].value;

                } else {
                    playerColor = pc.options[pc.selectedIndex].value;
                }

                // 各職業現金流起始值設定
                // Create object for each player with occupation scenario
                var playerObj = new APP.scenario(APP.scenarioChoices[playerScenario]);

                // Add player name to player objecti
                playerObj.name = APP.name(i);
                playerObj.color = playerColor;

                // Add player object to array of players
                APP.players.push(playerObj);

                // Send list of players to board
                var tableId = document.getElementById("player-list-table");
                // 原生 js 把傳入的字串解析成HTML 或XML，並把該節點插入到DOM 樹指定的位置
                tableId.insertAdjacentHTML(
                    "beforeend",
                    "<div class='table-row-player' id='table-row-player" +
                    parseInt(i, 10) +
                    "'> " +
                    playerObj.name +
                    " </div>"
                );
                // 更新玩家所選的顏色
                APP.display.updatePlayerColor('table row', i)

            }

            // 突出顯示第一個玩家
            // Highlight first player
            var curPlayerRowId = document.getElementById(
                "table-row-player" + parseInt(APP.currentPlayer, 10)
            );
            curPlayerRowId.style.border = "3pt groove #FDD835";

            // Set game variables
            // Included assets
            // Starting cash

            // 另一個 options 模組，在畫面上會在起始遊戲前選擇觸發 defaultOptions、selectGameMode
            OPTIONS.setup();

            // dream 模組
            // Start dream phase (phase 1)
            APP.dreamPhase.openDreamPhase();
            APP.dreamPhase.dreamPhaseOn = true;

            // 開始遊戲
            // Show game menu
            $("#game-menu").show();

            APP.display.clearBtns();
            APP.display.clearCards();
            APP.clearAmounts();

            $("#end-turn-btn").hide();
            $("#ft-end-turn-btn").hide();

            $("#opp-card-btns").hide();
            $("#buy-opp-button").hide();
            $("#doodad-pay-button").hide();
            $("#ds-pay-button").hide();
            $("#pd-pay-button").hide();
            $("#charity-donate-btn").hide();
            $("#done-btn").hide();
            $("#pass-button").hide();
            $("#roll2-btn").hide();
            $("#ft-roll2-btn").hide();
            $("#confirm-pay-btn").hide();
            $("#exp-child-row").hide();
            $("#ft-turn-instructions").hide();
            $("#ft-roll-btn").hide();
            $("#ft-dream-roll-btn").hide();
            $("#ft-doodad-roll-btn").hide();
            $("#ft-enter-btn").hide();

            $("#finish-instructions").hide();

        } else if (gameState == 'continue game') {
            load();

            for (var i = 0; i < APP.players.length; i++) {
                var tableId = document.getElementById("player-list-table");
                tableId.insertAdjacentHTML(
                    "beforeend",
                    "<div class='table-row-player' id='table-row-player" +
                    parseInt(i + 1, 10) +
                    "'> " +
                    APP.players[i]['name'] +
                    " </div>"
                );
            }

            $("#game-menu").show();

            APP.display.clearBtns();
            APP.display.clearCards();

            $("#end-turn-btn").hide();
            $("#ft-end-turn-btn").hide();

            $("#opp-card-btns").hide();
            $("#buy-opp-button").hide();
            $("#doodad-pay-button").hide();
            $("#ds-pay-button").hide();
            $("#pd-pay-button").hide();
            $("#charity-donate-btn").hide();
            $("#done-btn").hide();
            $("#pass-button").hide();
            $("#roll2-btn").hide();
            $("#ft-roll2-btn").hide();
            $("#confirm-pay-btn").hide();
            $("#exp-child-row").hide();
            $("#ft-turn-instructions").hide();
            $("#ft-roll-btn").hide();
            $("#ft-dream-roll-btn").hide();
            $("#ft-doodad-roll-btn").hide();
            $("#ft-enter-btn").hide();

            $("#finish-instructions").hide();

            APP.dreamPhase.endDreamPhase();
            APP.dreamPhase.dreamPhaseOn = false;

            APP.nextTurn('continued game');

            var curPlayerRowId = document.getElementById(
                "table-row-player" + parseInt(APP.currentPlayer, 10)
            );

            curPlayerRowId.style.border = "3pt groove #FDD835";
        }
    },
    // 擲骰
    rollDie: function (dieCount) {
        var dieTotal = 0;

        for (i = 1; i <= dieCount; i++) {
            var die = Math.floor(Math.random() * 6) + 1;
            dieTotal += die;
        }

        return dieTotal;
    },
    // 移動玩家棋子rolledDie的數量
    movePlayer: function (dieCount) {
        // Move player piece the amount of rolledDie
        var player = APP.currentPlayerArrPos();
        var pObj = APP.players[player];
        var previousPosition = pObj.position;
        var dice;
        var manualDice = document.getElementById("manual-dice-input");

        if (OPTIONS.manualDice.checked == true) {
            // 顯示骰子輸入
            //show dice input
            dice = manualDice.value;
        } else {

            dice = this.rollDie(dieCount);
        }

        // show rolled dice info
        $("#roll-info-container").show();
        $("#roll-info").text("Dice: " + String(dice));

        var token = APP.display.tokens[player];

        // 刪除舊片
        // Remove old piece
        var playerTokenEle = document.getElementById(
            ("player" + parseInt(APP.currentPlayer, 10) + "-piece")
        );

        playerTokenEle.remove();
        // 更新棋盤位置
        // Update board position
        this.updatePosition(dice);
        // 將令牌添加到新部分
        // Add token to new section
        var token = APP.display.tokens[player].ele;
        var currentPosition = pObj.position;
        var newSquare = document.getElementById(
            "tokenSection" + parseInt(currentPosition, 10)
        );
        $(token).appendTo(newSquare);
        // Color piece after move
        APP.display.colorGamePiece(APP.currentPlayer, pObj.color);

        // Add highlight to current player piece
        //playerTokenEle.style.boxShadow = '5px 5px 1px yellow';
        // Remove highlight from other pieces
        for (var i = 1; i < this.pCount; i++) {
            var playerPiece = "player" + parseInt(i, 10) + "-piece";
            if (i !== APP.currentPlayer) {
                //document.getElementById(playerPiece).style.boxShadow = '';
            }
        }

        // 當玩家降落在方形負載卡上時
        // When player lands on square load card
        APP.loadCard(currentPosition);

        // 如果通過薪水獲得發薪日 - 當前設置為薪水
        // If pass paycheck get payday - currently set to salary
        if (previousPosition < 5 && currentPosition >= 5) {
            pObj.cash += pObj.payday;
        } else if (previousPosition < 13 && currentPosition >= 13) {
            pObj.cash += pObj.payday;
        } else if (previousPosition < 21 && currentPosition + dice >= 21) {
            pObj.cash += pObj.payday;
        }

        // finance 金融模組 財務報表
        APP.finance.statement();
    },
    // 更新位置
    updatePosition: function (dice) {
        var p = APP.players[APP.currentPlayerArrPos()];

        if (p.position + dice <= 23) {
            p.position += dice;
        } else {
            var x = p.position + dice;
            x -= 23;
            p.position = x;
        }
    },
    nextTurn: function (gameState) {
        var player = APP.players[this.currentPlayerArrPos()];

        $("#finish-instructions").hide();
        $("#finish-turn-container").hide();
        $("#ft-end-turn-btn").hide();
        $("#ft-dream-roll-btn").hide(); //--
        $("#fast-track-intro-card").hide();
        $("#fast-track-option-card").hide();
        $("#roll-info-container").hide();

        APP.display.clearBtns();
        APP.display.clearCards();
        APP.clearAmounts();

        //remove card highlight
        $("#turn-info").css("border", "2pt solid transparent");
        $("#turn-info").css("box-shadow", "0 0 2px #212121");
        $(".card-title").css("text-shadow", ".2px .2px .2px #7DCEA0");
        $(".card-title").css("color", "#4E342E");

        // 隱藏上一個資產並顯示當前資產
        //hide prev assets and show current assets
        if (player.stockAssets.length >= 0) {
            var stockRowClass =
                ".stock-shares" + parseInt(APP.currentPlayerArrPos(), 10) + "-row";
            $(stockRowClass).hide();
        }
        if (player.realEstateAssets.length >= 0) {
            var rowClass =
                ".real-estate-asset" + parseInt(APP.currentPlayerArrPos(), 10) + "-row";
            $(rowClass).hide();
        }
        if (player.coinAssets.length >= 0) {
            var coinRowClass =
                ".coin-asset" + parseInt(APP.currentPlayerArrPos(), 10) + "-row";
            $(coinRowClass).hide();
        }

        if (player.fastTrack == false) {
            if (APP.dreamPhase.dreamPhaseOn == false) {
                $("#card-btns").show();
                $("#roll-btn").show();
                $("#menu-save-btn").show();
            } else {
                $("#menu-save-btn").hide();
                $("#roll-btn").hide();
            }

            $("#turn-instructions").show();

            $("#roll2-btn").hide();
            $("#ftic-ok-btn").hide();

            $("#asset-table").show();
            $("#liability-table").show();
            $("#ft-statement").hide();

            if (player.fastTrackOption == true) {
                $("#ft-enter-btn").show();
            } else {
                $("#ft-enter-btn").hide();
            }
        } else {
            $("#ft-turn-instructions").show();
            $("#ft-roll-btn").show();
            $("#ft-roll2-btn").hide();
            $("#ft-enter-btn").hide();

            // fast track statement
            $("#asset-table").hide();
            $("#liability-table").hide();
            $("#ft-statement").show();
        }

        if (APP.dreamPhase.dreamPhaseOn == true) {
            $("#turn-info-box").hide();
        }

        // 換 turn
        if (APP.currentPlayer < APP.pCount) {
            APP.currentPlayer++;
        } else {
            APP.currentPlayer = 1;
        }
        player = APP.players[APP.currentPlayerArrPos()];

        // 顯示其餘資產
        if (player.stockAssets.length >= 0) {
            var stockRowId =
                ".stock-shares" + parseInt(APP.currentPlayerArrPos(), 10) + "-row";
            $(stockRowId).show();
        }
        if (player.stockAssets.length >= 0) {
            var realEstateRowClass =
                ".real-estate-asset" + parseInt(APP.currentPlayerArrPos(), 10) + "-row";
            $(realEstateRowClass).show();
        }
        if (player.coinAssets.length >= 0) {
            var coinRowClass =
                ".coin-asset" + parseInt(APP.currentPlayerArrPos(), 10) + "-row";
            $(coinRowClass).show();
        }

        document.getElementById("player-name").innerHTML = APP.players[APP.currentPlayerArrPos()]['name'];
        document.getElementById("ft-player-name").innerHTML = APP.players[APP.currentPlayerArrPos()]['name'];

        APP.turnCount++;

        $("#turn-info--").html("Turn: " + APP.turnCount);

        // 捐款給慈善單位顯示特殊骰子
        if (player.charityTurns === 0) {
            $("#roll2-btn").hide();
            $("#ft-roll2-btn").hide();
        } else {
            if (player.fastTrack == true) {
                $("#ft-roll2-btn").show();
            } else {
                $("#roll2-btn").show();
            }
            player.charityTurns--;
        }

        // 不準動 ?
        if (player.downsizedTurns != 0) {
            player.downsizedTurns--;
            if (APP.pCount != 1) {
                this.nextTurn();
            }
        }

        if (gameState !== 'continued game') {
            if (APP.pCount == 1) {
                var player1RowId = document.getElementById("table-row-player1");
                player1RowId.style.border = "3pt grove #827717";
            } else {
                var curPlayerRowId = document.getElementById(
                    "table-row-player" + parseInt(APP.currentPlayer, 10)
                );
                var prevPlayerRowId = document.getElementById(
                    "table-row-player" + parseInt(APP.previousPlayer(), 10)
                );

                //highlight next player
                curPlayerRowId.style.border = "3pt groove #FDD835";
                prevPlayerRowId.style.border = "1pt double #F5F5F5";
            }
        }

        // 計算財務報表 我猜
        APP.finance.statement();

        // 檢查破產
        APP.checkBankruptcy();
    },
    // 結束這回合 ( 在每個卡片事件結束後 call )
    finishTurn: function () {
        // hide card
        $("#turn-instructions").hide();
        $("#cancel-btn").hide();
        $("#done-repay-btn").hide();
        $("#done-btn").hide();
        $("#offer-settlement").hide();
        $("#roll-info-container").hide();

        APP.display.clearCards();
        APP.display.clearBtns();

        // 顯示指令、銀行選項和結束回合 btn
        // show instructions, bank options, and end turn btn
        $("#finish-turn-container").show();
        $("#finish-instructions").show();
        $("#end-turn-btn").show();
        $("#repay-borrow-btns").show();

        // remove card highlight
        $("#turn-info").css("border", "2pt solid transparent");
        $("#turn-info").css("box-shadow", "0 0 2px #212121");
        $(".card-title").css("text-shadow", ".2px .2px .2px #7DCEA0");
        $(".card-title").css("color", "#4E342E");

        var player = APP.players[APP.currentPlayerArrPos()];
        var realEstateAssets = player.realEstateAssets;
        var coinAssets = player.coinAssets;

        if (realEstateAssets.length > 0) {
            for (var i = 0; i < realEstateAssets.length; i++) {
                realEstateAssets[i].highlight = "off";

                var rowId = "#asset" + parseInt(i, 10) + "-row";

                $(rowId).click(function () {
                    return 0;
                });
            }
        }
        if (coinAssets.length > 0) {
            for (var i = 0; i < coinAssets.length; i++) {
                coinAssets[i].highlight = false;

                var rowId = "#asset-c" + parseInt(i, 10) + "-row";

                $(rowId).click(function () {
                    return 0;
                });
            }
        }

        if (player.debt == false) {
            if (realEstateAssets.length > 0) {
                for (var i = 0; i < realEstateAssets.length; i++) {
                    var rowId = "#asset" + parseInt(i, 10) + "-row";

                    $(rowId).click(function () {
                        return 0;
                    });
                }
            }
        }

        // 計算財務報表
        APP.finance.statement();
    },
    // 額外東西雜事
    getDoodad: function () {
        var player = APP.players[APP.currentPlayerArrPos()];

        //random doodad
        var obj = APP.cards.doodad;
        var keys = Object.keys(obj);
        var randDoodad = function (object) {
            return object[keys[Math.floor(keys.length * Math.random())]];
        };
        var currentDoodad = randDoodad(obj);

        //check if doodad requires children
        if (currentDoodad.child == true && player.children == 0) {
            //get new doodad if player has none
            this.getDoodad();
        } else {
            this.currentDoodad = currentDoodad;
        }

        //set doodad
        var doodadName = this.currentDoodad.name;

        if (this.currentDoodad.amount) {
            this.currentDoodad.cost = player.cash * this.currentDoodad.amount;
        }
        var doodadCost = this.currentDoodad.cost;
        var text = this.currentDoodad.text;

        //if boat
        if (doodadName == "New Boat!" && player.boatLoan == 0) {
            player.boatLoan = 17000;
            player.boatPayment = 340;
        } else if (doodadName == "New Boat!") {
            text = "You already own one.";
        }

        //if credit card
        if (doodadName == "Buy Big Screen TV") {
            player.creditDebt = 4000;
            player.tvPayment = 120;
        }

        // 顯示額外東西雜事按鈕
        //display doodad
        document.getElementById("doodad-title").innerHTML = doodadName;
        document.getElementById("doodad-text").innerHTML = text;
    },
    // 取得報價
    getOffer: function () {
        var player = APP.players[APP.currentPlayerArrPos()];

        var obj = APP.cards.offer;
        var keys = Object.keys(obj);
        var randOffer = function (object) {
            return object[keys[Math.floor(keys.length * Math.random())]];
        };
        var currentOffer = randOffer(obj);

        this.currentOffer = currentOffer;
        this.currentOfferOffered = currentOffer.offer;

        document.getElementById("offer-title").innerHTML = currentOffer.name;
        document.getElementById("offer-description").innerHTML =
            currentOffer.description;
        document.getElementById("offer-rule1").innerHTML = currentOffer.rule1;
        document.getElementById("offer-rule2").innerHTML = currentOffer.rule2;

        // 如果玩家在其資產中具有該類型的屬性，則顯示出售/適當按鈕
        //if player has the type of property in their assets show sell/appropriate button
        var offerType = APP.currentOffer.type;

        var assetArr = player.realEstateAssets;
        var coinArr = player.coinAssets;

        APP.display.renderAssetTable();

        switch (offerType) {
            case "4-plex":
            case "8-plex":
            case "duplex":
                for (var i = 0; i < assetArr.length; i++) {
                    if (assetArr[i].landType == offerType) {
                        assetArr[i].highlight = "on";
                    }
                }
                break;
            case "plex":
                for (var i = 0; i < assetArr.length; i++) {
                    if (assetArr[i].landType == offerType || assetArr[i].landType == "duplex" || assetArr[i].landType == "4-plex" || assetArr[i].landType == "8-plex") {
                        assetArr[i].highlight = "on";
                    }
                }
                break;
            case "apartment":
                for (var i = 0; i < assetArr.length; i++) {
                    if (
                        assetArr[i].landType == offerType &&
                        assetArr[i].units > APP.currentOffer.lowestUnit
                    ) {
                        assetArr[i].highlight = "on";
                    }
                }
                break;
            case "bed breakfast":
            case "6Br/6Ba":
            case "5Br/4Ba":
            case "10 acres":
            case "20 acres":
            case "3Br/2Ba":
            case "2Br/1Ba":
                for (var i = 0; i < assetArr.length; i++) {
                    if (assetArr[i].landType == offerType) {
                        assetArr[i].highlight = "on";
                    }
                }
                break;
            case "3Br/2Ba+":
                //  3br2ba 成本增加 50000
                //add 50000 to 3br2ba costs
                for (var i = 0; i < assetArr.length; i++) {
                    if (assetArr[i].landType == "3Br/2Ba") {
                        assetArr[i].cost += 50000;
                    }
                }
                break;
            case "3Br/2Ba-":
                //Remove 3br2ba and cashFlow
                // 刪除 3br2ba 和 cashFlow
                for (var i = 0; i < assetArr.length; i++) {

                    if (assetArr[i].landType == "3Br/2Ba") {
                        player.assetIncome -= assetArr[i].cashFlow;

                        var index = assetArr.findIndex(x => x.landType == "3Br/2Ba");

                        assetArr.splice(index, 1);
                    }
                }
                break;
            case "limited":
                for (var i = 0; i < assetArr.length; i++) {
                    while (assetArr[i].landType == offerType) {
                        var settlement = assetArr[i].cost * 2;
                        //highlight
                        $("#turn-info").css("box-shadow", ".2px .2px 3px 3px #0277BD");

                        $("#offer-settlement").show();
                        document.getElementById("offer-settlement").innerHTML =
                            "Your Settlement: $" +
                            parseInt(settlement, 10) +
                            " per partnership";
                        APP.currentSettlement = settlement;
                        APP.currentSettlementCashFlow = assetArr[i].cashFlow;
                        APP.currentSettlementId = assetArr[i].id;
                        var id = APP.currentSettlementId;

                        player.cash += settlement;
                        player.assetIncome -= assetArr[i].cashFlow;

                        //remove from array
                        var index = assetArr.findIndex(x => x.id == id);
                        assetArr.splice(index, 1);
                    }
                }
                break;
            case "franchise":
                //unused
                break;
            case "business":
                var businessAssetArr = player.businessAssets;
                for (var i = 0; i < businessAssetArr.length; i++) {
                    if (businessAssetArr[i].landType == offerType) {
                        var cashFlow = APP.currentOffer.cashFlow;
                        businessAssetArr[i].cashFlow += cashFlow;
                        player.assetIncome += cashFlow;
                    }
                }
                break;
            case "car wash":
                for (var i = 0; i < assetArr.length; i++) {
                    if (assetArr[i].landType == offerType) {
                        assetArr[i].highlight = "on";
                    }
                }
                break;
            case "widget":
                break;
            case "mall":
                for (var i = 0; i < assetArr.length; i++) {
                    if (assetArr[i].landType == offerType) {
                        assetArr[i].highlight = "on";
                    }
                }
                break;
            case "Krugerrands":
                for (var i = 0; i < coinArr.length; i++) {
                    if (coinArr[i].name == "Krugerrands") {
                        coinArr[i].highlight = true;
                    }
                }
                break;
            case "1500's Spanish":
                for (var i = 0; i < coinArr.length; i++) {
                    if (coinArr[i].name == "1500's Spanish") {
                        coinArr[i].highlight = true;
                    }
                }
                break;
            default:

                break;
        }
    },
    // 結算
    getSettlement: function (row, debt) {
        var player = APP.players[APP.currentPlayerArrPos()];
        var currentId = row;

        currentId.split('');

        var index;

        if (currentId.length > 100) {
            index = Number(currentId[5] + currentId[6] + currentId[7]);
        } else if (currentId.length > 10) {
            index = Number(currentId[5] + currentId[6]);
        } else {
            index = Number(currentId[5]);
        }

        // 當前結算指數
        this.currentSettlementIndex = index;

        // 如果有債務或沒債務
        if (debt === true) {
            var bankruptcySettlement = player.realEstateAssets[index].downPayment / 2;

            APP.currentSettlement = bankruptcySettlement;
            APP.currentSettlementCashFlow = player.realEstateAssets[index].cashFlow;
            APP.currentSettlementId = player.realEstateAssets[index].id;

            $("#roll-btn").hide();

            $("#confirm-settlement-btn").show();
            $("#br-settlement-text").show();
            $("#br-settlement-offer").html(parseInt(bankruptcySettlement, 10));

        } else {
            switch (APP.currentOffer.type) {
                case "4-plex":
                case "8-plex":
                case "duplex":
                case "plex":
                    APP.currentSettlement =
                        player.realEstateAssets[index].units * APP.currentOffer.offerPerUnit -
                        player.realEstateAssets[index].mortgage;
                    APP.currentSettlementCashFlow = player.realEstateAssets[index].cashFlow;
                    break;
                case "apartment":
                    APP.currentSettlement =
                        player.realEstateAssets[index].units * APP.currentOffer.offerPerUnit -
                        player.realEstateAssets[index].mortgage;
                    APP.currentSettlementCashFlow = player.realEstateAssets[index].cashFlow;
                    break;
                    break;
                default:
                    APP.currentSettlement = APP.currentOfferOffered - player.realEstateAssets[index].mortgage;
                    APP.currentSettlementCashFlow = player.realEstateAssets[index].cashFlow;
                    break;
            }

            $("#offer-card").hide();
            $("#done-btn").hide();

            $("#settlement-card").show();
            $("#confirm-settlement-btn").show();
            $("#show-offer-btn").show();
            $("#settlement-offer").html(APP.display.numWithCommas(APP.currentSettlement));
        }
    },
    // 小交易
    smallDeal: function () {
        var player = APP.players[APP.currentPlayerArrPos()];

        //get random deal
        var obj = APP.cards.smallDeal;
        var keys = Object.keys(obj);
        var randDeal = function (object) {
            return object[keys[Math.floor(keys.length * Math.random())]];
        };
        const currentDeal = randDeal(obj);
        var dealType;

        if (!currentDeal) {
            dealType = "none";
        } else {
            this.currentDeal = currentDeal;
            dealType = currentDeal.type;
        }

        $("#opp-card").hide();
        $("#small-deal-btn").hide();
        $("#big-deal-btn").hide();
        $("#sell-shares-form").hide();

        APP.display.showCurrentDeal();
    },
    // 大交易
    bigDeal: function () {
        var player = APP.players[APP.currentPlayerArrPos()];
        var obj = APP.cards.bigDeal;
        var keys = Object.keys(obj);
        var randDeal = function (object) {
            return object[keys[Math.floor(keys.length * Math.random())]];
        };
        var currentDeal = randDeal(obj);
        var dealType;

        if (!currentDeal) {
            dealType = "none";
        } else {
            dealType = currentDeal.type;
            this.currentDeal = currentDeal;
        }

        $("#opp-card").hide();
        $("#small-deal-btn").hide();
        $("#big-deal-btn").hide();

        APP.display.showCurrentDeal();
    },
    // 清除金額用於下一回合與新遊戲開始時
    clearAmounts: function () {
        APP.finance.loanAmount = 1000;
        APP.finance.mortgagePrepay = false;

        delete APP.currentDeal;
        delete APP.currentDoodad;

        delete APP.currentOffer;
        delete APP.currentSettlement;
        delete APP.currentSettlementId;
        delete APP.currentOfferOffered;

        document.getElementById("loan-amt-input").value = 1000;
        document.getElementById("loan-amt-input2").value = 1000;
        document.getElementById("share-amt-input").value = 1;
        document.getElementById("share-amt-input-sell").value = 1;
    },
    // 擁有股份
    ownedShares: function () {
        var player = APP.players[APP.currentPlayerArrPos()];
        var arr = player.stockAssets;
        //var stockId = APP.currentDeal.id;
        var stockSymbol = APP.currentDeal.symbol;
        var shares = 0;

        if (arr.length > 0) {
            for (var i = 0; i < arr.length; i++) {
                if (arr[i].symbol == stockSymbol) {
                    shares += Number(arr[i].shares);
                }
            }
            return shares;
        } else {
            return 0;
        }
    },
    // 檢查破產
    checkBankruptcy: function (amountOwed) {
        var player = APP.players[APP.currentPlayerArrPos()];
        var propertyAssets = player.realEstateAssets;
        var businessAssets = player.businessAssets;
        var coinAssets = player.coinAssets;
        var stockAssets = player.stockAssets;

        if (player.payday < 0) {
            player.loanApproval = false;
        } else {
            player.loanApproval = true;
        }

        /*	triggered by
            - finance statement

            triggered when
            - when player owes money
                - pay for doodad
                - pay day

            what happens
            - bankruptcy card
            - sell assets
        */

        /* 由觸發- 財務報表

                     觸發時
                     - 當玩家欠錢時
                         - 支付小飾品
                         - 發薪日

                     怎麼了
                     - 破產卡
                     - 出售資產
                 */

        if (player.payday < 0 && player.cash < 0) {
            player.debt = true;

            //clear cards
            APP.display.clearCards();
            APP.display.clearBtns();

            // if the player has no assets to sell they lose the game, else allow selling assets at half the downpayment
            // 如果玩家沒有資產可以出售，他們就輸了，否則允許以一半的首付出售資產

            /*if ((player.realEstateAssets.length &&
                        player.businessAssets.length &&
                        player.coinAssets.length &&
                        player.stockAssets.length
                    ) ^ includes selling all assets for bankruptcy*/

            if (player.realEstateAssets.length < 1) {
                player.debtSale = false;

                $("#bankrupt-game-over-card").show();
                $("#bankrupt-card").hide();
                $("#roll-btn").hide();
                $("#roll2-btn").hide();
                // continue button

                APP.finance.statement();
            } else if (player.realEstateAssets.length > 0) {
                //if player has assets
                player.debtSale = true;

                $("#bankrupt-card").show();
                $("#br-cash-flow").html(String(APP.display.numWithCommas(player.cash)));
                $("#br-settlement-text").hide();
                $("#roll-btn").hide();
                $("#roll2-btn").hide();

                APP.finance.statement();
            }
        } else if (0 < (player.payday || player.cash)) {
            player.debt = false;

            if (APP.currentDeal == true) {
                $("#return-to-card-btn").show();
            }
        } else {
            player.debt = false;
        }
    }
};

// 重點模組 財務模組
APP.finance = {
    statement: function () {
        // get current player
        var player = APP.players[APP.currentPlayerArrPos()];

        // Income 收入
        document.getElementById("player-job-income").innerHTML = APP.display.numWithCommas(player.jobTitle[0]);
        document.getElementById("player-salary-income").innerHTML = APP.display.numWithCommas(player.jobTitle[1]);

        // Expenses 花費
        APP.finance.getTaxes();
        document.getElementById("expenses-taxes").innerHTML = APP.display.numWithCommas(player.jobTitle[3]);
        document.getElementById("expenses-mortgage").innerHTML = APP.display.numWithCommas(player.jobTitle[4]);
        document.getElementById("expenses-car").innerHTML = APP.display.numWithCommas(player.jobTitle[5]);
        document.getElementById("expenses-credit").innerHTML = APP.display.numWithCommas(player.jobTitle[6]);
        document.getElementById("expenses-retail").innerHTML = APP.display.numWithCommas(player.jobTitle[7]);
        document.getElementById("expenses-other").innerHTML = APP.display.numWithCommas(player.jobTitle[8]);
        this.loanPayment(APP.currentPlayerArrPos());
        document.getElementById("child-count").innerHTML = player.children;
        document.getElementById("expenses-loans").innerHTML = APP.display.numWithCommas(player.loanPayment);

        if (player.boatLoan > 0) { // 船
            $("#exp-boat-row").show();
            document.getElementById("expenses-boatloan").innerHTML = APP.display.numWithCommas(player.boatPayment);
        } else {
            $("#exp-boat-row").hide();
        }
        if (player.insurance > 0) { // 保險
            $("#exp-insurance-row").show();
            document.getElementById("expenses-insurance").innerHTML = APP.display.numWithCommas(player.insurance);
        } else {
            $("#exp-insurance-row").hide();
        }

        // Summary
        if (APP.players[APP.currentPlayerArrPos()].hasInsurance == true) {
            this.getInsurance(APP.currentPlayerArrPos()); // 獲得保險
        }
        this.getExpenses(APP.currentPlayerArrPos()); // 獲取費用
        this.getIncome(APP.currentPlayerArrPos()); // 獲得收入
        this.getPayday(APP.currentPlayerArrPos()); // 發薪日

        // 如果玩家處於快車道，則顯示獲勝所需的金額
        // Show amount needed to win if player is in the fast track
        if (player.fastTrack == true) {
            // 隱藏總費用並顯示winpay金額
            //hide total expenses and show winpay amount
            $("#total-expenses-header").hide();
            $("#win-pay-header").show();
            document.getElementById("summary-win-bar").innerHTML = APP.display.numWithCommas(player.winPay);
            document.getElementById("bar-passive-income").innerHTML = APP.display.numWithCommas(player.cashFlowDay);
        } else {
            $("#total-expenses-header").show();
            $("#win-pay-header").hide();
            // 顯示總費用
            //show total expenses
            document.getElementById("summary-total-expenses").innerHTML = APP.display.numWithCommas(player.totalExpenses);
            document.getElementById("summary-total-expenses-bar").innerHTML = APP.display.numWithCommas(player.totalExpenses);
            document.getElementById("bar-passive-income").innerHTML = APP.display.numWithCommas(player.passiveIncome);
        }

        // summary totals
        document.getElementById("summary-cash").innerHTML = APP.display.numWithCommas(Math.round(player.cash));
        document.getElementById("summary-total-income").innerHTML = APP.display.numWithCommas(player.totalIncome);
        document.getElementById("summary-payday").innerHTML = APP.display.numWithCommas(player.payday);

        // get asset table 獲取資產表
        APP.display.renderStockTable();
        APP.display.renderAssetTable();

        // get liabilities table 獲取負債表
        APP.display.renderLiabilitiesTable();

        // amount needed for fast track progress bar 快速跟踪進度條所需的數量
        this.progressBar();

        var expenseBarEle = document.getElementById("income-expense-bar");

        if (expenseBarEle.style.width == "100%") {
            if (player.fastTrackOption == false) {
                player.fastTrackOption = true;

                APP.display.clearCards();
                APP.display.clearBtns();

                $("#fast-track-intro-card").show();

                $("#turn-instructions").hide();
                $("#finish-instructions").hide();
                $("#ft-turn-instructions").hide();
                $("#roll-btn").hide();
                $("#end-turn-btn").hide();
                $("#fast-track-option-card").hide();

                document.getElementById("ftic-player-name").innerHTML = APP.name(APP.currentPlayer);
                document.getElementById("ftic-player-name-intro").innerHTML = APP.name(APP.currentPlayer);
                $("#ftic-ok-btn").show();
                $("#ft-enter-btn").show();
            }
        }

        //Check for Fast Track 檢查快速通道
        if (player.fastTrack == true) {
            APP.display.renderFtAssets();
            $("#income-table").hide();
            $("#liability-table").hide();
            $("#sum-total-expense-row").hide();
            $("#sum-total-income-row").hide();
            $("#asset-statement").css("width", "90%");

            console.log("cashflow day: " + player.cashFlowDay + ", winpay: " + player.winPay);

            if (player.cashFlowDay >= player.winPay) {
                FASTTRACK.winGame();
            }
        } else {
            $("#sum-total-expense-row").show();
            $("#sum-total-income-row").show();
            $("#income-table").show();
            $("#liability-table").show();

            if (player.realEstateAssets.length >= 5) {
                $("#income-table").css("height", "14%");
                $("#asset-stock-body").css("height", "300px");
            } else {
                $("#income-table").css("height", "10%");
                $("#asset-stock-body").css("height", "16%");
            }

            $("#asset-statement").css("width", "98%");
        }
    },
    // 進度條
    progressBar: function () {
        var player = APP.players[APP.currentPlayerArrPos()];
        var expenseBarEle = document.getElementById("income-expense-bar");
        var expenses = this.getExpenses(APP.currentPlayerArrPos()); // 獲取費用
        var width;

        if (player.fastTrack == true) {
            width = 100 * (player.fastTrackIncome / player.winPay);

            if (width > 100) {
                expenseBarEle.style.width = "100%";
            } else {
                expenseBarEle.style.width = Math.round(width) + "%";
            }
        } else {
            width = 100 * (player.passiveIncome / expenses); // 被動收入

            if (width > 100) {
                expenseBarEle.style.width = "100%";
            } else {
                expenseBarEle.style.width = Math.round(width) + "%";
            }
        }

        if (width > 100) {
            expenseBarEle.style.width = "100%";
        } else {
            expenseBarEle.style.width = Math.round(width) + "%";
        }
    },
    // 獲得收入
    getIncome: function (currentPlayer) {
        var player = APP.players[currentPlayer];
        var salary = player.jobTitle[1]; // 薪水
        var dividends = 0; // 股息
        var assetIncome = 0; // 資產收益
        var fastTrackIncome = 0; // 快速通道收入

        var stockArr = player.stockAssets; // 庫存
        var realEstateArr = player.realEstateAssets; // 房地產
        var businessArr = player.businessAssets; // 商業資產
        var ftArr = player.fastTrackAssets; // 快速通道資產

        // get income from stocks, assets and businesses 從股票、資產和業務中獲得收入
        for (var i = 0; i < stockArr.length; i++) {
            if (stockArr[i].type == "Preferred Stock" || stockArr[i].type == "Certificate of Deposit") {
                var stockReturn = stockArr[i].shares * stockArr[i].dividend;

                dividends += stockReturn;
            }
        }
        for (var i = 0; i < realEstateArr.length; i++) {
            if (realEstateArr[i].cashFlow) {
                assetIncome += realEstateArr[i].cashFlow;
            }
        }
        for (var i = 0; i < businessArr.length; i++) {
            if (businessArr[i].cashFlow) {
                assetIncome += businessArr[i].cashFlow;
            }
        }
        for (var i = 0; i < ftArr.length; i++) {
            if (ftArr[i].cashFlow) {
                fastTrackIncome += ftArr[i].cashFlow;
            }
        }

        // get total income 獲得總收入
        if (player.fastTrack == false) {
            player.totalIncome = salary + /*player.assetIncome +*/ assetIncome + dividends;
            player.passiveIncome = assetIncome + dividends;
        } else {
            player.totalIncome = player.cashFlowDay + fastTrackIncome;
        }

        return player.totalIncome;
    },
    // 獲取費用
    getExpenses: function (currentPlayer) {
        //total expenses = liabilities + bills 總費用 = 負債 + 賬單
        var player = APP.players[currentPlayer];

        var taxes = APP.players[currentPlayer].jobTitle[3]; // 稅
        var mortgage = APP.players[currentPlayer].jobTitle[4]; // 抵押貸款
        var car = APP.players[currentPlayer].jobTitle[5]; // 車貸
        var credit = APP.players[currentPlayer].jobTitle[6]; // 信用卡
        var retail = APP.players[currentPlayer].jobTitle[7]; // 零售
        var other = APP.players[currentPlayer].jobTitle[8]; // 雜費
        var children = player.childExpense;
        var loanPayment = player.loanPayment; // 貸款
        var boatPayment = player.boatPayment; // 船貸

        var insurance = player.insurance; // 保險

        if (player.hasInsurance == true) {
            this.getInsurance(currentPlayer);
        }

        if (player.fastTrack == false) {
            player.totalExpenses =
                taxes +
                mortgage +
                car +
                credit +
                retail +
                other +
                children +
                loanPayment +
                boatPayment +
                insurance;
        } else {
            player.totalExpenses = 0;
        }

        return player.totalExpenses;
    },
    // 獲得發薪日 ( 收入扣花費 )
    getPayday: function (currentPlayer) {
        var player = APP.players[currentPlayer];
        var income = this.getIncome(currentPlayer); // 獲得收入
        var expenses = this.getExpenses(currentPlayer); // 獲取費用
        var pay = income - expenses;
        if (player.fastTrack == false) {
            player.payday = pay;
        } else {
            player.payday = player.cashFlowDay + income;
        }
    },
    // 獲取稅收
    getTaxes: function () {
        // 基於 2019 年美國聯邦所得稅等級
        //based on 2019 United States federal income tax brackets
        var player = APP.players[APP.currentPlayerArrPos()];
        var taxes = player.jobTitle[3];

        if (6875 < player.totalIncome && player.totalIncome < 13084) {
            taxes = player.totalIncome * .24;
        } else if (13084 < player.totalIncome && player.totalIncome < 16667) {
            taxes = player.totalIncome * .32;
        } else if (16667 < player.totalIncome && player.totalIncome < 41667) {
            taxes = player.totalIncome * .35;
        } else if (41667 < player.totalIncome) {
            taxes = player.totalIncome * .37;
        } else {
            taxes = player.totalIncome * .22;
        }

        player.jobTitle[3] = Math.round(taxes);
        return Math.round(taxes);
    },
    // 獲得保險
    getInsurance: function (player) {
        //player income
        var curPlayer = APP.players[player];
        var income = this.getIncome(player);

        //pay a base 8% and 1% for every dependent 為每個受撫養人支付基本 8% 和 1%
        if (curPlayer.children > 0) {
            curPlayer.insurance = Math.round(income * (0.08 + (0.01 * APP.players[player].children)));

        } else {
            curPlayer.insurance = Math.round(income * (0.08));

        }

        return curPlayer.insurance;
    },
    // 小玩意
    payDoodad: function () {
        var player = APP.players[APP.currentPlayerArrPos()];

        if (APP.currentDoodad.name == "New Boat") {
            player.boatLoan = 17000;
            player.boatPayment = 340;
            APP.finishTurn();
        } else if (APP.currentDoodad.cost <= player.cash) {
            player.cash -= APP.currentDoodad.cost;
            APP.finishTurn();
        } else {
            this.loanOffer(APP.currentDoodad.cost);
        }
    },
    // 裁員
    payDownsize: function () {
        //show liability card 出示責任卡
        var player = APP.players[APP.currentPlayerArrPos()];
        var boardPosition = player.position;
        var downsizedAmount = player.totalExpenses;

        if (player.hasInsurance == true) {
            APP.finishTurn();
        } else {
            if (player.cash < downsizedAmount) {
                this.loanOffer(downsizedAmount);
            } else {
                player.cash -= downsizedAmount;
                player.downsizedTurns += 3;
                APP.finishTurn();
            }
        }
    },
    // 賠償財產損失
    payPropertyDamage: function () {
        var player = APP.players[APP.currentPlayerArrPos()];
        var cost = APP.currentDeal.cost;

        if (player.cash < cost) {
            this.loanOffer(cost);
        } else {
            player.cash -= cost;
            APP.finishTurn();
        }
    },
    // 捐款
    donate: function () {
        var dPlayer = APP.players[APP.currentPlayerArrPos()];
        var donation = dPlayer.totalIncome * 0.1;

        if (dPlayer.cash >= donation) {
            dPlayer.cash = dPlayer.cash - donation;
            dPlayer.charityTurns += 3;
            if (dPlayer.fastTrack == true) {
                FASTTRACK.finishTurn();
            } else {
                APP.finishTurn();
            }
        } else {
            $("#charity-donate-btn").hide();
            $("#charity-card").hide();
            $("#pass-btn").hide();
            $("#cannot-afford-card").show();
            $("#done-repay-btn").show();
        }
    },
    // 增加貸款
    increaseLoan: function () {
        var player = APP.players[APP.currentPlayerArrPos()];
        var value1 = parseInt(document.getElementById("loan-amt-input").value, 10);
        var value2 = parseInt(document.getElementById("loan-amt-input2").value, 10);
        var loan = parseInt(document.getElementById("loan-amt-input2").value, 10);

        //value1 = isNaN(value1) ? 0 : value1;
        value1 += 1000;
        document.getElementById("loan-amt-input").value = value1;

        if (loan < player.loans) {

            //value2 = isNaN(value2) ? 0 : value2;

            if (value2 + 1000 > player.cash) {
                value2 += 0;
            } else {
                value2 += 1000;
            }
            document.getElementById("loan-amt-input2").value = value2;
        }
    },
    // 減少貸款
    decreaseLoan: function () {
        var value = parseInt(document.getElementById("loan-amt-input").value, 10);

        value = isNaN(value) ? 0 : value;
        if (value > 1000) {
            value -= 1000;
        } else {
            value = 1000;
        }
        document.getElementById("loan-amt-input").value = value;

        var value2 = parseInt(document.getElementById("loan-amt-input2").value, 10);
        value2 = isNaN(value2) ? 0 : value2;
        if (value2 > 1000) {
            value2 -= 1000;
        } else {
            value2 = 0;
        }
        document.getElementById("loan-amt-input2").value = value2;
    },
    // 拿出貸款
    takeOutLoan: function () {
        var loan = parseInt(document.getElementById("loan-amt-input").value, 10);
        var player = APP.players[APP.currentPlayerArrPos()];
        loan = isNaN(loan) ? 0 : loan;

        player.loans += loan;
        player.cash += loan;

        APP.finishTurn();
    },
    // 償還貸款
    repayLoan: function () {
        var loan = parseInt(document.getElementById("loan-amt-input2").value, 10);
        var player = APP.players[APP.currentPlayerArrPos()];
        loan = isNaN(loan) ? 0 : loan;

        //if player can afford load 如果玩家負擔得起
        if (player.loanId == "liability-mortgage") {
            player.cash -= loan;
            player.jobTitle[9] -= loan;

            if (player.jobTitle[9] <= 0) {
                player.jobTitle[4] = 0;
                player.jobTitle[9] = 0;
            }
        } else if (player.loanId == "liability-boat") {
            player.cash -= loan;
            player.boatLoan -= loan;

            if (player.boatLoan <= 0) {
                player.boatLoan = 0;
                player.boatPayment = 0;
            }
        } else {
            player.loans -= loan;
            player.cash -= loan;
        }

        $("#confirm-pay-btn").hide();
        APP.display.highlightLiabilities(2);
    },
    // 貸款支付
    loanPayment: function (currentPlayer) {
        var player = APP.players[currentPlayer];
        var loanPayment = player.loans * 0.1;
        player.loanPayment = loanPayment;
        return loanPayment;
    },
    // 支付
    pay: function () {
        var player = APP.players[APP.currentPlayerArrPos()];
        var loanId = player.loanId;

        switch (loanId) {
            case "liability-mortgage": // 負債抵押
                if (this.mortgagePrepay == true) {
                    $("#cancel-btn").hide();
                    $("#pay-confirm-card").hide();
                    $("#confirm-pay-btn").hide();

                    $("#done-repay-btn").show();
                    $("#repay-card").show();

                    this.repayLoan();
                    APP.finishTurn();

                    document.getElementById("loan-amt-input2").value = 1000;
                } else if (player.cash < player.jobTitle[9]) {
                    $("#confirm-pay-btn").hide();
                    $("#pay-loan-confirmation").html("Not enough funds");
                } else {
                    player.cash -= player.jobTitle[9];
                    player.jobTitle[4] = 0;
                    player.jobTitle[9] = 0;

                    $("#cancel-btn").hide();
                    $("#pay-confirm-card").hide();
                    $("#confirm-pay-btn").hide();

                    $("#done-repay-btn").show();
                    $("#repay-card").show();
                }
                break;
            case "liability-car":
                if (player.cash < player.jobTitle[10]) {
                    $("#confirm-pay-btn").hide();
                    $("#pay-loan-confirmation").html("Not enough funds");
                } else {
                    player.cash -= player.jobTitle[10];
                    player.jobTitle[5] = 0;
                    player.jobTitle[10] = 0;

                    $("#pay-confirm-card").hide();
                    $("#confirm-pay-btn").hide();
                    $("#cancel-btn").hide();

                    $("#done-repay-btn").show();
                    $("#repay-card").show();
                }
                break;
            case "liability-credit":
                if (player.cash < player.jobTitle[11]) {
                    $("#confirm-pay-btn").hide();
                    $("#pay-loan-confirmation").html("Not enough funds");
                } else {
                    player.cash -= player.jobTitle[11];
                    player.jobTitle[6] = 0;
                    player.jobTitle[11] = 0;

                    $("#repay-card").show();
                    $("#done-repay-btn").show();

                    $("#confirm-pay-btn").hide();
                    $("#cancel-btn").hide();
                    $("#pay-confirm-card").hide();
                }
                break;
            case "liability-retail":
                if (player.cash < player.jobTitle[12]) {
                    $("#confirm-pay-btn").hide();
                    $("#pay-loan-confirmation").html("Not enough funds");
                } else {
                    player.cash -= 1000;
                    player.jobTitle[7] = 0;
                    player.jobTitle[12] = 0;

                    $("#repay-card").show();
                    $("#done-repay-btn").show();

                    $("#cancel-btn").hide();
                    $("#pay-confirm-card").hide();
                    $("#repay-loan-card").hide();
                    $("#confirm-pay-btn").hide();
                }
                break;
            case "liability-loans":
                var loanAmt = document.getElementById("loan-amt-input2").value;
                if (player.cash < loanAmt) {
                    $("#confirm-pay-btn").hide();
                    $("#pay-loan-confirmation").html("Not enough funds");
                } else if (loanAmt == 0) {
                    $("#repay-card").hide();
                    $("#pay-confirm-card").hide();
                    $("#confirm-pay-btn").hide();

                    $("#done-repay-btn").show();
                    $("#repay-card").show();

                } else {
                    this.repayLoan();
                    APP.finishTurn();
                }
                document.getElementById("loan-amt-input2").value = 1000;
                break;
            case "liability-boat":
                $("#cancel-btn").hide();
                $("#pay-confirm-card").hide();
                $("#confirm-pay-btn").hide();

                $("#done-repay-btn").show();
                $("#repay-card").show();

                this.repayLoan();
                APP.finishTurn();

                document.getElementById("loan-amt-input2").value = 1000;
                break;
            default:
                APP.finishTurn();
                break;
        }
        APP.finance.statement();
    },
    // 購買股票
    buyStock: function () {
        var player = APP.players[APP.currentPlayerArrPos()];

        APP.currentDeal.shares = 0;
        APP.currentDeal.shares += Number(
            document.getElementById("share-amt-input").value
        );

        const stockObj = JSON.parse(JSON.stringify(APP.currentDeal));

        stockObj.selected = false;

        var shares = Number(stockObj.shares);
        var price = Number(stockObj.price);
        var cost = price * shares;
        var stockId = stockObj.id;
        var arr = player.stockAssets;
        var index = arr.findIndex(x => x.id == stockId);

        if (cost <= player.cash) {
            player.cash -= cost;

            if (index == -1) {
                arr.push(stockObj);
            } else {
                arr[index].shares += shares;
            }

            $("#show-stock-form-btn").show();
            $("#show-stock-sell-form-btn").show();
            $("#buy-shares-form").hide();
            $("#buy-stock-btn").hide();
            $("#done-buy-sell-btn").hide();
            $("#done-btn").show();
        } else {
            $("#show-stock-form-btn").hide();
            $("#show-stock-sell-form-btn").hide();
            $("#buy-stock-btn").hide();
            $("#done-btn").hide();
            $("#done-buy-sell-btn").hide();

            this.loanOffer(cost);
        }

        if (APP.currentDeal == true && APP.ownedShares() > 0) {
            $("#show-stock-sell-form-btn").show();
        }
        document.getElementById(
            "deal-stock-shares-owned"
        ).innerHTML = APP.ownedShares();

        APP.finance.statement();
    },
    // 賣出股票
    sellStock: function () {
        //--
        var player = APP.players[APP.currentPlayerArrPos()];
        var arr = player.stockAssets;

        APP.currentDeal.shares = 0;
        APP.currentDeal.shares += Number(
            document.getElementById("share-amt-input-sell").value
        );

        let sellStockObj = JSON.parse(JSON.stringify(APP.currentDeal));

        var shares = Number(sellStockObj.shares);
        var price = Number(sellStockObj.price);
        var re = price * shares;
        var index;

        for (var i = 0; i < arr.length; i++) {
            if (arr[i].selected === true) {
                index = i;
            }
        }

        player.cash += re;
        arr[index].shares -= shares;

        document.getElementById(
            "deal-stock-shares-owned"
        ).innerHTML = APP.ownedShares();

        $("#show-stock-form-btn").show();
        $("#show-stock-sell-form-btn").show();
        $("#sell-shares-form").hide();
        $("#sell-stock-btn").hide();
        $("#done-buy-sell-btn").hide();
        $("#done-btn").show();

        if (arr[index].shares === 0) {
            arr.splice(index, 1);
            if (arr.length === 0) {
                $("#show-stock-sell-form-btn").hide();
            }
        }
        for (var j = 0; j < arr.length; j++) {
            if (arr[j].highlight === "on") {
                arr[j].highlight = "off";
            }
            if (arr[j].selected === true) {
                arr[j].selected = false;
            }
        }
        APP.finance.statement();
    },
    // 股票分割
    stockSplit: function (type) {
        var player = APP.players[APP.currentPlayerArrPos()];
        var arr = player.stockAssets;
        var stockSymbol = APP.currentDeal.symbol;

        for (var i = 0; i < arr.length; i++) {
            var shares = arr[i].shares;
            if (arr[i].symbol == stockSymbol) {
                if (type == "split") {
                    arr[i].shares = shares * 2;
                    $("#turn-info").css("box-shadow", ".2px .2px 3px 3px #43A047");
                } else if (type == "reverse") {
                    arr[i].shares = shares / 2;
                    $("#turn-info").css("box-shadow", ".2px .2px 3px 3px#D32F2F");
                }
            }
        }
    },
    // 購買房地產
    buyRealEstate: function () {
        var player = APP.players[APP.currentPlayerArrPos()];
        var downPayment = APP.currentDeal.downPayment;
        var arr = player.realEstateAssets;

        if (downPayment <= player.cash) {
            player.cash -= downPayment;

            APP.currentDeal.id = this.newId();

            arr.push(JSON.parse(JSON.stringify(APP.currentDeal)));

            APP.finishTurn();
        } else if (downPayment > player.cash) {
            this.loanOffer(downPayment);
        }
    },
    // 購買硬幣
    buyCoin: function () {
        var player = APP.players[APP.currentPlayerArrPos()];
        var cost = APP.currentDeal.cost;
        var name = APP.currentDeal.name;
        var arr = player.coinAssets;
        var index = arr.findIndex(x => x.name == name);

        // if player can afford coin, buy. if not offer loan 如果玩家買得起硬幣，請購買。如果不提供貸款
        if (cost <= player.cash) {
            player.cash -= cost;
            //check if player already owns coins 檢查玩家是否已經擁有硬幣
            if (index in arr) {
                if (cost == 500) {
                    arr[index].amount += 1;
                } else if (cost == 3000) {
                    arr[index].amount += 10;
                }
            } else {
                arr.push(JSON.parse(JSON.stringify(APP.currentDeal)));
            }

            APP.finishTurn();
        } else {
            this.loanOffer(cost);
        }
    },
    // 購買業務
    buyBusiness: function () {
        var player = APP.players[APP.currentPlayerArrPos()];
        var downPayment = APP.currentDeal.downPayment;
        var arr = player.businessAssets;
        var businessAsset = JSON.parse(JSON.stringify(APP.currentDeal));

        if (downPayment <= player.cash) {
            player.cash -= downPayment;

            var type = businessAsset.businessType;
            var tag = APP.currentDeal.landType;
            var idNum = Math.random() * (9999999 - 1000000) + 1000000;
            var newId = tag + idNum;
            businessAsset.id = newId;

            arr.push(businessAsset);

            APP.finishTurn();
        } else {
            this.loanOffer(downPayment);
        }
    },
    // 出售資產
    sellAsset: function () {
        //Settlement = Sales Price – RE Mortgage 結算 = 銷售價格 -可再生能源抵押
        var player = APP.players[APP.currentPlayerArrPos()];
        var assetArr = player.realEstateAssets;
        var coinArr = player.coinAssets;

        if (player.debt == true) {
            player.cash += APP.currentSettlement;
            player.assetIncome -= APP.currentSettlementCashFlow;

            assetArr.splice(APP.currentSettlementIndex, 1);

            $("#confirm-settlement-btn").hide();

            //if player cash is high enough to pay for doodad 如果玩家現金足夠支付小飾品
            if (APP.currentDoodad) {
                if (APP.currentDoodad.cost < player.cash) {
                    $("#return-to-card-btn").show();
                }
            }

            //if downsize 裁員
            if (player.position == 19) {
                if (APP.finance.getIncome(APP.currentPlayerArrPos()) < player.cash) {
                    $("#return-to-card-btn").show();
                }
            } else {
                //if payday or any spot not an opp space 如果發薪日或任何地點不是 opp 空間
                if (0 < player.cash) {
                    $("#return-to-card-btn").show();
                }
            }
        } else {
            $("#confirm-settlement-btn").hide();
            $("#settlement-card").hide();
            $("#show-offer-btn").hide();

            if (APP.currentOffer.type && (APP.currentOffer.type == "Krugerrands" || APP.currentOffer.type == "1500's Spanish")) {
                var type = APP.currentOffer.type;
                var index = assetArr.findIndex(x => x.name == type);

                player.cash += APP.settlementOffer;

                coinArr.splice(index, 1);
            } else

            /*if (APP.currentOffer.type == ("4-plex" || "8-plex"
            || "duplex" || "plex" || "apartment" || "bed breakfast" || "10 acres" || "20 acres" || "3Br/2Ba" || "2Br/1Ba"))*/ {
                player.cash += APP.currentSettlement;
                player.assetIncome -= APP.currentSettlementCashFlow;

                assetArr.splice(APP.currentSettlementIndex, 1);
            }

            if (APP.currentOffer) {
                $("#offer-card").show();
            }
        }

        if (player.debt == false) {
            $("#done-btn").show();
        }

        APP.finance.statement();
    },
    loanAmount: 1000, // 貸款額度
    // 貸款優惠
    loanOffer: function (cost) {
        var player = APP.players[APP.currentPlayerArrPos()];
        const amountToCover = cost;
        this.roundLoan(amountToCover);
        const loan = this.loanAmount;

        APP.display.clearCards();
        APP.display.clearBtns();

        $("#ft-enter-btn").hide();

        //update to check if player has income for loan 更新以檢查玩家是否有貸款收入

        player.loanApproval = true;

        $("#cannot-afford-loan-card").show();
        $("#borrow-offer-loan-btn").show();
        $("#loan-offered-text").show();

        //remove card title color
        if (player.position === 2 % 0 || player.position === 0) {
            $(".card-title").css("color", "#4E342E");
        }

        document.getElementById("loan-offer").innerHTML = loan;
        document.getElementById("loan-offer-monthly-payment").innerHTML =
            loan * 0.1;

        if (player.position == (1 || 9 || 17)) {
            $("#no-loan-btn").hide();
        } else {
            $("#no-loan-btn").show();
        }

        if (player.position === 19) {
            $("#no-loan-btn").hide();
        }

        //--temp
        if (APP.currentDeal == true && APP.ownedShares() == 0) {
            $("#show-stock-sell-form-btn").hide();
            $("#done-btn").show();
        }

        APP.finance.statement();
    },
    // 循環貸款
    roundLoan: function (cost) {
        var player = APP.players[APP.currentPlayerArrPos()];
        var val = cost - player.cash;

        if (val < 1000) {
            this.loanAmount = 1000;
        } else {
            this.loanAmount = Math.ceil(val / 1000) * 1000;
        }
    },
    // 獲得貸款
    getLoan: function () {
        var player = APP.players[APP.currentPlayerArrPos()];
        var boardPosition = player.position;
        var downsizedAmount = player.totalExpenses;

        player.loans += this.loanAmount;
        player.cash += this.loanAmount;

        $("#cannot-afford-loan-card").hide();
        $("#borrow-offer-loan-btn").hide();
        $("#no-loan-btn").hide();

        APP.display.returnToCard();
    },
    // 沒有貸款
    noLoan: function () {
        var player = APP.players[APP.currentPlayerArrPos()];
        var boardPosition = player.position;
        var downsizedAmount = player.totalExpenses;

        $("#cannot-afford-loan-card").hide();
        $("#borrow-offer-loan-btn").hide();
        $("#no-loan-btn").hide();

        //return to card
        APP.display.returnToCard();
    },
    mortgagePrepay: false,
    newId: function () {
        return Math.round(Math.random() * (9999999 - 1000000) + 1000000);
    }
};

// 依據格子類別讀取卡片
APP.loadCard = function (boardPosition) {
    var playerObj = APP.players[APP.currentPlayerArrPos()];
    //hide turn instructions
    $("#turn-instructions").hide();
    $("#ft-turn-instructions").hide();
    $("#repay-borrow-btns").hide();
    $("#roll-btn").hide();
    $("#roll2-btn").hide();
    $("#ft-roll-btn").hide();
    $("#ft-roll2-btn").hide();
    $("#end-turn-btn").hide();
    $("#ft-end-turn-btn").hide();
    $("#confirm-pay-btn").hide();
    $("#ft-enter-btn").hide();
    $("#fast-track-intro-card").hide();

    if (playerObj.fastTrack == true) {
        var currentSquare = "square" + String(boardPosition);
        var doodadTitle = document.getElementById("ft-doodad-title");
        var doodadText = document.getElementById("ft-doodad-text");

        // show fast track finance statement
        switch (boardPosition) {
            // Doodad spaces
            case 1:
                $("#ft-doodad-card").show();
                $("#ft-doodad-roll-btn").show();

                $(".card-title").css("color", "#D32F2F");

                doodadTitle.innerHTML = FASTTRACK.square.doodad1.title;

                FASTTRACK.doodad(boardPosition);
                break;
            case 7:
                $("#ft-doodad-card").show();
                $("#ft-end-turn-btn").show();
                $(".card-title").css("color", "#D32F2F");

                doodadTitle.innerHTML = FASTTRACK.square.doodad2.title;
                doodadText.innerHTML = FASTTRACK.square.doodad2.text;

                FASTTRACK.doodad(boardPosition);
                break;
            case 14:
                $("#ft-doodad-card").show();
                $("#ft-end-turn-btn").show();
                $(".card-title").css("color", "#D32F2F");

                FASTTRACK.doodad(boardPosition);

                doodadTitle.innerHTML = FASTTRACK.square.doodad3.title;
                doodadText.innerHTML = FASTTRACK.square.doodad3.text;
                break;
            case 21:
                $("#ft-doodad-card").show();
                $("#ft-end-turn-btn").show();
                $(".card-title").css("color", "#D32F2F");

                FASTTRACK.doodad(boardPosition);

                doodadTitle.innerHTML = FASTTRACK.square.doodad4.title;
                doodadText.innerHTML = FASTTRACK.square.doodad4.text + " - " + FASTTRACK.square.doodad4.lowestAsset;
                break;
            case 27:
                $("#ft-doodad-card").show();
                $("#ft-end-turn-btn").show();
                $(".card-title").css("color", "#D32F2F");

                FASTTRACK.doodad(boardPosition);

                doodadTitle.innerHTML = FASTTRACK.square.doodad5.title;
                doodadText.innerHTML = FASTTRACK.square.doodad5.text;
                break;
            case 34:
                $("#ft-doodad-card").show();
                $("#ft-end-turn-btn").show();
                $(".card-title").css("color", "#D32F2F");

                FASTTRACK.doodad(boardPosition);

                doodadTitle.innerHTML = FASTTRACK.square.doodad6.title;
                doodadText.innerHTML = FASTTRACK.square.doodad6.text;
                break;
            // CashFlow Day
            case 10:
            case 18:
            case 30:
            case 38:
                $("#ft-cashflow-day").show();
                $("#ft-end-turn-btn").show();

                $("#ft-cashflow-day-income").text(APP.display.numWithCommas(playerObj.payday));
                break;

            // Charity space
            case 2:
                $("#charity-card").show();
                $("#charity-donate-btn").show();
                $("#ft-end-turn-btn").show();

                $(".card-title").css("color", "#00BCD4");
                break;

            //Opportunity spaces
            case 17:
                $("#ft-deal-retun-row").hide();
                $("#ft-opp-buy-btn").hide();
                $("#ft-opp-prompt").hide();

                $("#ft-opp-card").show();
                $("#ft-deal-cash-flow-row").show();
                $("#ft-opp-roll").show();
                $("#ft-pass-btn").show();
                $("#ft-deal-cost-table").show();

                $(".card-title").css("color", "#43A047");

                if (playerObj.cash < FASTTRACK.square[currentSquare].cost) {
                    $("#ft-opp-buy-btn").hide();
                } else {
                    $("#ft-opp-buy-btn").show();
                }

                $("#ft-opp-title").html(FASTTRACK.square[currentSquare].title);
                $("#ft-card-text").html(FASTTRACK.square[currentSquare].text);
                $("#ft-deal-return").html(FASTTRACK.square[currentSquare].returnText);
                $("#ft-deal-cash-flow").html(FASTTRACK.square[currentSquare].cashFlowText);
                $("#ft-deal-cost").html(FASTTRACK.square[currentSquare].costText);
                break;
            case 23:
                $("#ft-deal-cash-flow-row").hide();

                $("#ft-opp-card").show();
                $("#ft-deal-retun-row").show();
                $("#ft-deal-cost-table").show();
                $("#ft-opp-roll").show();
                $("#ft-pass-btn").show();

                $(".card-title").css("color", "#43A047");

                if (playerObj.cash < FASTTRACK.square[currentSquare].cost) {
                    $("#ft-opp-buy-btn").hide();
                } else {
                    $("#ft-opp-buy-btn").show();
                }

                $("#ft-opp-title").html(FASTTRACK.square[currentSquare].title);
                $("#ft-card-text").html(FASTTRACK.square[currentSquare].text);
                $("#ft-deal-return").html(FASTTRACK.square[currentSquare].returnText);
                $("#ft-deal-cost").html(FASTTRACK.square[currentSquare].costText);
                break;
            case 33:
                $("#ft-deal-cash-flow-row").hide();

                $("#ft-opp-card").show();
                $("#ft-deal-retun-row").show();
                $("#ft-opp-roll").show();
                $("#ft-pass-btn").show();
                $("#ft-deal-cost-table").show();

                $(".card-title").css("color", "#43A047");

                if (playerObj.cash < FASTTRACK.square[currentSquare].cost) {
                    $("#ft-opp-buy-btn").hide();
                } else {
                    $("#ft-opp-buy-btn").show();
                }

                $("#ft-opp-title").html(FASTTRACK.square[currentSquare].title);
                $("#ft-card-text").html(FASTTRACK.square[currentSquare].text);
                $("#ft-deal-return").html(FASTTRACK.square[currentSquare].returnText);
                $("#ft-deal-cost").html(FASTTRACK.square[currentSquare].costText);
                break;

            // Dream space
            case 24:
                $("#ft-deal-cash-flow-row").hide();
                $("#ft-deal-return-row").hide();
                $("#ft-deal-cost-table").hide();

                $("#ft-opp-card").show();
                $("#ft-dream-roll-btn").show();
                $(".card-title").css("color", "#FDD835");

                $("#ft-opp-title").html(FASTTRACK.square[currentSquare].title);
                $("#ft-card-text").html(FASTTRACK.square[currentSquare].text);
                $("#ft-deal-cost").html(FASTTRACK.square[currentSquare].costText);
                break;
            default:
                $("#ft-deal-return-row").hide();
                $("#ft-opp-prompt").hide();

                $("#ft-opp-card").show();
                $("#ft-deal-cash-flow-row").show();
                $("#ft-deal-cost-table").show();

                $("#ft-pass-btn").show();

                if (playerObj.cash < FASTTRACK.square[currentSquare].cost) {
                    $("#ft-opp-buy-btn").hide();
                } else {
                    $("#ft-opp-buy-btn").show();
                }

                $(".card-title").css("color", "#43A047");

                $("#ft-opp-title").html(FASTTRACK.square[currentSquare].title);
                $("#ft-card-text").html(FASTTRACK.square[currentSquare].text);
                $("#ft-deal-cash-flow").html(FASTTRACK.square[currentSquare].cashFlowText);
                $("#ft-deal-cost").html(FASTTRACK.square[currentSquare].costText);
                break;
        }
    } else if (playerObj.fastTrack == false) {
        //opportunity
        if (boardPosition % 2 === 0 || boardPosition === 0) {
            // if mixed deal option is on
            $("#opp-card").show();

            $("#card-btns").show();
            $("#small-deal-btn").show();
            $("#big-deal-btn").show();

            //highlight title
            $(".card-title").css("color", "#43A047");
        }

        // doodad aka expense
        if (boardPosition === 1 || boardPosition === 9 || boardPosition === 17) {
            $("#doodad-card").show();
            $("#doodad-pay-button").show();
            APP.getDoodad();

            //highlight title
            $(".card-title").css("color", "#D32F2F");
        }

        //offer
        if (boardPosition === 7 || boardPosition === 15 || boardPosition === 23) {
            $("#offer-card").show();
            $("#done-btn").show();
            APP.getOffer();

            //highlight title
            $(".card-title").css("color", "#0277BD");
        }

        //paycheck
        if (boardPosition === 5 || boardPosition === 13 || boardPosition === 21) {
            if (OPTIONS.paycheckDoodads.checked == true) {
                $("#doodad-card").show();
                $("#doodad-pay-button").show();
                APP.getDoodad();

                //highlight title
                $(".card-title").css("color", "#D32F2F");
            } else {
                this.finishTurn();
            }
        }

        //charity
        if (boardPosition === 3) {
            $("#charity-card").show();
            $("#charity-donate-btn").show();
            $("#pass-btn").show();

            //highlight title
            $(".card-title").css("color", "#00BCD4");
        }

        //kid
        if (boardPosition === 11) {
            $("#kid-card").show();
            $("#done-btn").show();
            var expense = Math.round(parseInt(playerObj.totalIncome) * 0.056);
            var eTable = document.getElementById("expense-table");

            //highlight card
            //$("#turn-info").css("border", "2pt solid #FFB74D");
            //$("#turn-info").css("box-shadow", "2px 2px 4px 4px #FFB74D");
            $(".card-title").css("color", "#FFB74D");

            if (playerObj.children == 0) {
                //add row to expenses table
                $("#exp-child-row").show();
                document.getElementById("child-cost").innerHTML = expense;
                //children count ++
                playerObj.children += 1;
            } else if (playerObj.children == 3 && playerObj.kidLimit == true) {
                //keep same kid count
            } else {
                playerObj.children += 1;
            }
            var children = playerObj.children;
            var childCost = children * expense;
            playerObj.childExpense = childCost;
            document.getElementById("expenses-child").innerHTML = APP.display.numWithCommas(playerObj.childExpense);

            APP.finance.statement(APP.currentPlayerArrPos());
        }

        //downsize
        if (boardPosition === 19) {
            var downsizedAmount = playerObj.totalExpenses;
            //show downsize card
            $("#downsize-card").show();

            //highlight card
            $(".card-title").css("color", "#D32F2F");

            if (playerObj.hasInsurance == false) {
                //show pay button
                $("#insured-ds").hide();
                $("#default-ds").show();
                $("#ds-pay-button").show();

                $("#turn-info").css("box-shadow", ".2px .2px 3px 3px #D32F2F");

                document.getElementById("downsized-amt").innerHTML = APP.display.numWithCommas(downsizedAmount);
            } else {
                $("#default-ds").hide();
                $("#insured-ds").show();
                $("#done-btn").show();

                document.getElementById("downsized-amt2").innerHTML = APP.display.numWithCommas(downsizedAmount);

                playerObj.downsizedTurns += 3;
            }
        }
    }
};

// 夢想階段
APP.dreamPhase = {
    dreamPhaseOn: true,
    dreamArrPos: 0,
    // 夢想設定檔
    openDreamPhase: function () {
        this.dreamPhaseOn = true;

        // 顯示夢想的標題和信息
        // show dream title and info
        var dream = document.getElementById("dream-text");
        var dreamDescription = document.getElementById("dream-des");

        dream.innerHTML = APP.dreamPhase.dreams[APP.dreamPhase.dreamArrPos];
        dreamDescription.innerHTML =
            APP.dreamPhase.dreamDescriptions[APP.dreamPhase.dreamArrPos];

        // 顯示球員的工作、收入和儲蓄
        // show player job, income and savings
        $("#job-text").show();

        // 顯示開始場景
        this.showStartScenario(0);
    },
    // 顯示開始場景
    showStartScenario: function (player) {
        var player = APP.players[APP.currentPlayerArrPos()];
        var playerJob = player.jobTitle[0];
        var vowelRegex = "^[aieouAIEOU].*";
        var matched = playerJob.match(vowelRegex);
        if (matched) {
            // 如果工作以元音開頭，則在職位前添加一個 n 和一個空格
            //add an n and a space before job title if job begins with vowel
            var job = "n " + playerJob;
        } else {
            //add space before jobtitle
            var job = " " + playerJob;
        }

        document.getElementById("dream-job").innerHTML = job;
        document.getElementById("dream-starting-salary").innerHTML = APP.display.numWithCommas(player.jobTitle[1]);
        document.getElementById("dream-starting-savings").innerHTML = APP.display.numWithCommas(player.cash);
        document.getElementById("dream-starting-cash").innerHTML = APP.display.numWithCommas(player.cash);
    },
    leftDream: function () {
        var id = document.getElementById("dream-text");
        var desId = document.getElementById("dream-des");

        if (this.dreamArrPos === 0) {
            this.dreamArrPos = APP.dreamPhase.dreams.length - 1;
        } else {
            this.dreamArrPos--;
        }

        id.innerHTML = APP.dreamPhase.dreams[APP.dreamPhase.dreamArrPos];
        desId.innerHTML = APP.dreamPhase.dreamDescriptions[this.dreamArrPos];
    },
    rightDream: function () {
        var id = document.getElementById("dream-text");
        var desId = document.getElementById("dream-des");

        if (this.dreamArrPos === APP.dreamPhase.dreams.length - 1) {
            this.dreamArrPos = 0;
        } else {
            this.dreamArrPos++;
        }

        id.innerHTML = APP.dreamPhase.dreams[this.dreamArrPos];
        desId.innerHTML = APP.dreamPhase.dreamDescriptions[this.dreamArrPos];
    },
    // 綁定夢想
    dreamChoiceBtn: function () {
        //save dream
        var chosenDream = this.dreams[this.dreamArrPos];
        APP.players[APP.currentPlayerArrPos()].dream = chosenDream;
        //start race phase once last player has dream
        if (APP.currentPlayer == APP.pCount) {
            this.endDreamPhase();
            $("#card-btns").show();
        }
        //show first dream
        var restartDream = document.getElementById("dream-text");
        restartDream.innerHTML = APP.dreamPhase.dreams[0];

        APP.nextTurn();
        //show job and savings info
        APP.dreamPhase.showStartScenario(APP.currentPlayerArrPos()); //+ 1
    },
    // 隱藏夢想選單
    endDreamPhase: function () {
        APP.display.hideDreamPhase();
        APP.display.showRacePhase();
        APP.dreamPhase.dreamPhaseOn = false;
        $("#job-text").show();
        $("#finance-box").show();
    },
    dreams: [
        "STOCK MARKET FOR KIDS 兒童股票市場",
        "YACHT RACING 遊艇比賽",
        "CANNES FILM FESTIVAL 戛納電影節",
        "PRIVATE FISHING CABIN ON A MONTANA LAKE 蒙大拿湖上的私人釣魚小屋",
        "PARK NAMED AFTER YOU 以你命名的公園",
        "RUN FOR MAYOR 競選市長",
        "GIFT OF FAITH 信心的恩賜",
        "HELI SKI THE SWISS ALPS 直升機滑雪瑞士阿爾卑斯山",
        "DINNER WITH THE PRESIDENT 與總統共進晚餐",
        "RESEARCH CENTER FOR CANCER AND AIDS 癌症和艾滋病研究中心",
        "7 WONDERS OF THE WORLD 世界七大奇蹟",
        "SAVE THE OCEAN MAMMALS 拯救海洋哺乳動物",
        "BE A JET SETTER 做一個噴氣式飛機二傳手",
        "GOLF AROUND THE WORLD 全球高爾夫",
        "A KIDS LIBRARY 兒童圖書館",
        "SOUTH SEA ISLAND FANTASY 南海島奇幻",
        "CAPITALISTS PEACE CORPS 資本家和平隊",
        "CRUISE THE MEDITERRANEAN 巡航地中海",
        "MINI FARM IN THE CITY 城市裡的迷你農場",
        "AFRICAN PHOTO SAFARI 非洲攝影之旅",
        "BUY A FOREST 購買森林",
        "PRO TEAM BOX SEATS 專業團隊包廂座位",
        "ANCIENT ASIAN CITIES 亞洲古代城市"
    ],
    dreamDescriptions: [
        "Fund a business and investment school for young capitalists, teaching the the basics of business. School includes a mini stock exchange run by the students. 資助一所面向年輕資本家的商業和投資學校，教授商業基礎知識。 學校包括一個由學生經營的小型證券交易所。",
        "You and your crew fly to Perth, Australia. Spend one week racing a 12-meter against the fastest boats in the world.你和你的船員飛往澳大利亞珀斯。花一周時間與世界上最快的船進行 12 米的比賽。",
        "Party with the stars! Tour France, plus one week in Cannes rubbing elbows with celebrities. You even land a starring role!與明星聚會！環遊法國，在戛納與名人擦肩而過一周。你甚至獲得了一個主演的角色！",
        "Fish from the dock of the remote cabin. Enjoy 6 months of solitude. Use of float plane included.從偏遠小屋的碼頭釣魚。享受 6 個月的孤獨。包括使用水上飛機。",
        "Tear down an abandoned warehouse and build a new recreational park. Donate police sub-station for park safety.拆除廢棄倉庫，新建遊樂公園。捐贈派出所，保障公園安全。",
        "Your financial expertise spurs masses of people to beg you to lead the city. You run and, of course, win. This is the start of your Presidential race.你的金融專業知識促使人們請求你領導這座城市。你跑了，當然，贏了。這是你總統競選的開始。",
        "Your religious organization is growing by leaps and bounds. New buildings are needed.你們的宗教組織正在突飛猛進地發展。需要新的建築。",
        "A winter of helicopter skiing by day and playing at the glamorous hot spots at night. A medieval castle is your accomodation.一個冬天的白天直升機滑雪，晚上在迷人的熱點玩耍。中世紀的城堡是你的住宿。",
        "Buy a table for 10 friends to dine with te President at a gala ball for visiting dignitaries from around the world.為 10 位朋友購買一張餐桌，與總統在一個盛大的舞會上共進晚餐，為來自世界各地的來訪貴賓。",
        "Your money brings together top researchers & doctors in one place, dedication to eliminating these two diseases.您的資金將頂級研究人員和醫生聚集在一個地方，致力於消除這兩種疾病。",
        "Go by plane, boat, bicycle, camel, canoe & limo to the 7 Wonders of the World. First class luxury all the way 乘飛機、船、自行車、駱駝、獨木舟和豪華轎車去世界七大奇蹟。一路頭等艙",
        "Fund and be a crew member on a month-long research expedition to protect endangered sea animals.資助並成為為期一個月的研究探險的船員，以保護瀕臨滅絕的海洋動物。",
        "Have your own personal jet available for one year to whisk you away whenever and wherever your heart desires.擁有您自己的私人飛機一年，隨時隨地帶您離開。",
        "You take 3 friends on a first-class, 5-star resort tour to play the 50 best golf courses in the world.你帶 3 位朋友參加一流的 5 星級度假村之旅，去玩世界上 50 個最好的高爾夫球場。",
        "Add a wing to your city's library devoted to young writers and artists. Art celebrities visit often to support your work.為您所在城市的圖書館增加一個專門為年輕作家和藝術家服務的側翼。藝術名人經常訪問以支持您的工作。",
        "Pampered in luxury for two full months. Relax, unwind in warm waters, deserted beaches, and romantic nights.整整兩個月的奢華享受。在溫暖的海水、荒涼的海灘和浪漫的夜晚放鬆身心。",
        "Set up entrepreneurial business schools in 3rd world nations. Instructors are business people donating their knowledge & time.在第三世界國家建立創業商學院。導師是商人，他們貢獻了他們的知識和時間。",
        "Visit small harbors in Italy, France, and Greece for a month with 12 friends on your private yacht.在您的私人遊艇上與 12 位朋友一起遊覽意大利、法國和希臘的小港口一個月。",
        "Create a hands-on farm eco-system for city kids to learn and care for animals and plants.為城市孩子們創造一個動手實踐的農場生態系統，讓他們學習和照顧動植物。",
        "Take 6 friends on a wild safari photographing the most exotic animals in the world. Enjoy 5-star luxury in your tent.帶 6 位朋友參加野外探險，拍攝世界上最奇特的動物。在您的帳篷裡享受 5 星級奢華。",
        "Stop the loss of ancient trees. Donate 1,000 acres of forest and create a nature walk for all to enjoy.杜絕古樹流失。捐出1000畝森林，打造人人共享的自然步道。",
        "License a 12 person private skybox booth with food and beverage service at your favorite team's stadium.在您最喜歡的球隊的體育場為 12 人私人天空包廂提供餐飲服務的許可。",
        "A private plane and guide take you and 5 friends to the most remote spots of Asia... where no tourists have gone before.一架私人飛機和導遊帶你和 5 位朋友前往亞洲最偏遠的地方……以前沒有遊客去過的地方。"
    ]
};

APP.scenario = function (
    jobTitle, // 職稱
    startingSalary, // 起始薪水
    startingSavings, // 起始儲蓄金額
    taxes, // 稅收
    mortgagePayment, // 抵押支付
    carPayment, // 汽車支付
    creditCardPayment, // 信用卡支付
    retailPayment, // 零售支付
    otherExpenses, // 其他費用
    mortgage, // 抵押
    carLoan, // 車貸
    creditDebt, // 信用債務
    retailDebt // 零售債務
) {
    this.jobTitle = jobTitle;
    this.startingSalary = startingSalary;
    this.startingSavings = startingSavings;
    this.taxes = taxes;
    this.mortgagePayment = mortgagePayment;

    this.carPayment = carPayment;
    this.creditCardPayment = creditCardPayment;
    this.retailPayment = retailPayment;
    this.otherExpenses = otherExpenses;
    this.cash = 0;

    this.mortgage = mortgage;
    this.carLoan = carLoan;
    this.creditDebt = creditDebt;
    this.retailDebt = retailDebt;
    this.position = 0;

    this.charityTurns = 0;
    this.childExpense = 0;
    this.children = 0;
    this.name = APP.name(APP.currentPlayer);
    this.totalIncome = 0;

    this.totalExpenses = 0;
    this.payday = 0;
    this.assetIncome = 0;
    this.passiveIncome = 0;
    this.loans = 0;

    this.loanPayment = 0;
    this.boatLoan = 0;
    this.boatPayment = 0;
    this.downsizedTurns = 0;
    this.stockAssets = [];

    this.realEstateAssets = [];
    this.businessAssets = [];
    this.coinAssets = [];
    this.personalAssets = [];
    this.fastTrack = false;

    this.fastTrackOption = false;
    this.cashFlowDay = 0;
    this.insurance = 0;
    this.hasInsurance = false;
    this.fastTrackAssets = [];

    this.loanApproval = true;
    this.mortgagePrepay = false;
};

// 各職業起始值：職稱、起始薪水、起始儲蓄金額、稅收、抵押支付、汽車支付、信用卡支付、零售支付、其他費用、抵押、車貸、信用債務、零售債務
APP.scenarioChoices = [
    // 民航員
    ["Airline Pilot", 9500, 400, 2350, 1300, 300, 660, 50, 2210, 143000, 15000, 22000, 1000],
    // 業務經理
    ["Business Manager", 4600, 400, 910, 700, 120, 90, 50, 1000, 75000, 6000, 3000, 1000],
    // 醫學博士
    ["Doctor (MD)", 13200, 400, 3420, 1900, 380, 270, 50, 2880, 202000, 19000, 9000, 1000],
    // 工程師
    ["Engineer", 4900, 400, 1050, 700, 140, 120, 50, 1090, 75000, 7000, 4000, 1000],
    // 守衛
    ["Janitor", 1600, 560, 280, 200, 60, 60, 50, 300, 20000, 4000, 2000, 1000],
    // 律師
    ["Lawyer", 7500, 400, 1830, 1100, 220, 180, 50, 1650, 115000, 11000, 6000, 1000],
    // 機械
    ["Mechanic", 2000, 670, 360, 300, 60, 60, 50, 450, 31000, 3000, 2000, 1000],
    // 護士
    ["Nurse", 3100, 480, 600, 400, 100, 90, 50, 710, 47000, 5000, 3000, 1000],
    // 警察
    ["Police Officer", 3000, 520, 580, 400, 100, 60, 50, 690, 46000, 5000, 2000, 1000],
    // 秘書
    ["Secretary", 2500, 710, 460, 400, 80, 60, 50, 570, 38000, 4000, 2000, 1000],
    // 教師
    ["Teacher (K-12)", 3300, 400, 630, 500, 100, 90, 50, 760, 50000, 5000, 3000, 1000],
    // 卡車司機
    ["Truck Driver", 2500, 750, 460, 400, 80, 60, 50, 570, 38000, 4000, 2000, 1000],
    // 首席執行長
    ["CEO", 24000, 60000, 7200, 1900, 800, 250, 50, 4200, 750000, 30000, 11000, 1000]
];

$(document).ready(function () {


    // init game
    APP.initGame();

    $("#menu-new-game-btn").on("click", function () {
        //new game
        window.scrollTo(0, 0)
        window.location.reload(false);
        //APP.display.newGame();
    });
    $("#menu-save-btn").on("click", function () {
        save(APP.saveKey);
    });
    $("#show-done-btn").on("click", function () {
        var doneBtn = document.getElementById("done-btn");
        if ($(doneBtn).css('display') === 'none') {
            $(doneBtn).show();
        } else {
            $(doneBtn).hide();
        }
    });
    $("#custom-game-indicator").text("Custom");

    /*var h, w, f;
    $(".hoverText").hover(function(){
        //get element id of hovered space
        console.log("id: " + this.id);

        h = document.getElementById(this.id).style.height;
        w = document.getElementById(this.id).style.width;
        f = document.getElementById(this.id).style.fontSize;

        document.getElementById(this.id).style.height = "200px";
        document.getElementById(this.id).style.width = "200px";
        document.getElementById(this.id).style.fontSize = "1.9em";

    },function(){
        document.getElementById(this.id).style.height = h;
        document.getElementById(this.id).style.width = w;
        document.getElementById(this.id).style.fontSize = f;
    });*/

    //stock form
    /*$("#share-amt-input-sell", "#decrease-shares-sell", "#increase-shares-sell").on("change", function(){
        var price = APP.currentDeal.price;
        var shares = this.value;

        document.getElementById("share-sell-total").innerHTML = price * shares;
    });*/

    // Options
    OPTIONS.output.innerHTML = "Normal";
    $("#default-game-indicator").css("color", "#FDD835");
    $("#custom-game-indicator").css("color", "#4E342E");
    $(".off-options").hide(); //-- hides unnused option menu checkboxes

    // listen for default settings on each change
    for (var i = 0; i < OPTIONS.checkbox.length; i++) {
        OPTIONS.checkbox[i].addEventListener('change', function () {
            OPTIONS.checkState();
        });
    }

    OPTIONS.slider.oninput = function () {
        if (this.value == 0) {
            OPTIONS.output.innerHTML = "None";
        } else if (this.value == 1) {
            OPTIONS.output.innerHTML = "Normal";
        } else if (this.value == 2) {
            OPTIONS.output.innerHTML = "Salary";
        } else if (this.value == 3) {
            OPTIONS.output.innerHTML = "2x Salary";
        }
        OPTIONS.checkState();
    };

    // 顯示選定數量的玩家的玩家選項，更新每個輸入
    // Show player options for selected amount of players, updates each input
    var playerInputs = document.querySelectorAll("div.player-input");

    function showPlayerInputs() {
        var val = OPTIONS.playerNumber.value;
        var options = document.querySelectorAll("option.player-number-option");

        for (var i = 1; i <= options.length; i++) {
            var playerInputBox = "#player" + i + "input";
            if (i <= val) {
                $(playerInputBox).show();
            } else {
                $(playerInputBox).hide();
            }
        }
    }

    OPTIONS.playerNumber.oninput = function () {
        var val = OPTIONS.playerNumber.value;
        var options = document.querySelectorAll("option.player-number-option");

        for (var i = 1; i <= options.length; i++) {
            var playerInputBox = "#player" + i + "input";
            if (i <= val) {
                $(playerInputBox).show();
            } else {
                $(playerInputBox).hide();
            }
        }
    }
    showPlayerInputs();
});

//update to support multiple saves, create save states
function save() {
    var saveState = {
        players: APP.players,
        pCount: APP.pCount,
        turnCount: APP.turnCount,
        currentPlayer: APP.currentPlayer,
        dreamPhaseOn: APP.dreamPhase.dreamPhaseOn
    };

    localStorage.setItem('saveState', JSON.stringify(saveState));
    localStorage.setItem('player array', JSON.stringify(APP.players));

    //alert
    document.getElementById("alert").innerHTML = 'Game Saved!';
    //remove alert
    setTimeout(function () { document.getElementById("alert").innerHTML = ''; }, 4000);
}

function load(saveKey) {
    var gameState = JSON.parse(localStorage.getItem('saveState'));
    var players = JSON.parse(localStorage.getItem('player array'));

    APP.dreamPhase.dreamPhaseOn = gameState['dreamPhaseOn'];
    APP.players = players;
    APP.pCount = gameState['pCount'];
    APP.turnCount = gameState['turnCount'];
    APP.currentPlayer = gameState['currentPlayer'];
}

function deleteSave(saveKey) {
    localStorage.removeItem('saveState');
    localStorage.removeItem('player array');
    $("#continue-game").css("display", "none");
    $("#start-game").css("padding", "6% 33.5%");
}

if (localStorage.getItem("saveState")) {
    $("#continue-game").css("display", "inline-block");
    $("#start-game").css("padding", "6% 8%");
    $("#start-game").css("margin", "auto");
    $("#delete-save").css("display", "block");
} else {
    $("#continue-game").css("display", "none");
    $("#delete-save").css("display", "none");
    $("#start-game").css("padding", "6% 33.5%");
}
