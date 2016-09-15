Bridge.assembly("Test", function ($asm, globals) {
    "use strict";

    Bridge.define("Test.App", {
        $main: function () {
            Test.Form.setup();

            var butBing = Bridge.merge(document.createElement('button'), {
                innerHTML: "Bing",
                onclick: $_.Test.App.f1
            } );

            var butNote = Bridge.merge(document.createElement('button'), {
                innerHTML: "NotePad",
                onclick: $_.Test.App.f2
            } );
            Test.Form.getWindowHolder().appendChild(butBing);
            Test.Form.getWindowHolder().appendChild(butNote);
        }
    });

    var $_ = {};

    Bridge.ns("Test.App", $_);

    Bridge.apply($_.Test.App, {
        f1: function (ev) {
            var frm = new Test.FormBrowser();
            frm.setLeft("50px");
            frm.setTop("50px");
            frm.setText("Bing");
            frm.navigate("http://bing.com");
            frm.show();
        },
        f2: function (ev) {
            var frm = new Test.FormNotePad();
            frm.setLeft("50px");
            frm.setTop("50px");
            frm.setText("Note Pad");
            frm.show();
        }
    });

    Bridge.define("Test.Form", {
        statics: {
            resizeCorners: 3,
            movingForm: null,
            parent: null,
            _ActiveForm: null,
            _PrevActiveForm: null,
            mouse_Down: false,
            moveAction: 0,
            WinIcon: "url('data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAADAAAAAoCAIAAAA35e4mAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAACSSURBVFhH7dbRCYAgFIXhRnASN3ADJ3GSu4gbuIGD1SUlejCOBpLE+R4NOT/0UJtZDIMQBiEMQhiEMAj5b5C11nsfQhCRlFLOeT/Vx93eBDnndFuHY4w6rCdlu6lc6TccVHdumoeXcqsfgxAGIcNBs/GVIQxCGIQMB6m1Pq5Pvvz9mIpBCIMQBiEMQhiELBZkzAGoRY/1a8YOvQAAAABJRU5ErkJggg==')",
            WinIconHover: "url('data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAADAAAAAoCAIAAAA35e4mAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAACmSURBVFhH7dYxCoQwEIVhb5NasNBGZCstBUFkL7Dg9ttq6QG8gJ2FB/I2DkS2EOUlghjkfUwVCfODhXrKMQxCGIQwCGEQwiDkuUF+GEdp8arq7NOU7fDupu84y6yPjZ0JCpJMdsvi/NfLYjnRu3dHXzFnHbTZJ7N7+B99yxyDEAYh1kFX4ytDGIQwCLEOEm59XI/c+ftxKQYhDEIYhDAIYRDiWJBSC3edj/DGIv8/AAAAAElFTkSuQmCC')",
            WinIconDown: "url('data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAADAAAAAoCAIAAAA35e4mAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAACnSURBVFhHY5AZZGDUQYTAqIMIgVEHEQKjDiIERh1ECAxfBynrGGvbehv6JFnGVrmUznWvXRE27zoQQaWJBuQ4SN3UHmg30GLHvIlAi4EiELuxIogW4gHJDkKzD4iwCsIRRBfxYNRBhMCogwgBkh1EazAaZYTAqIMIgVEHEQIkOwgIBlfligsMZPODpmDUQYTAqIMIgVEHEQKjDiIERh1ECAwyB8nIAADHEJbDMY47rQAAAABJRU5ErkJggg==')",
            visibleForm: null,
            config: {
                properties: {
                    TaskBar: null,
                    WindowHolder: null,
                    ButtonStart: null,
                    InputStartSearch: null
                },
                init: function () {
                    this.visibleForm = new (System.Collections.Generic.List$1(Test.Form))();
                }
            },
            getActiveForm: function () {
                return Test.Form._ActiveForm;
            },
            setActiveForm: function (value) {
                if (!Bridge.referenceEquals(Test.Form._ActiveForm, value)) {
                    Test.Form._PrevActiveForm = Test.Form._ActiveForm;

                    if (Test.Form._ActiveForm != null) {
                        if (Test.Form._ActiveForm.getBase() != null) {
                            Test.Form._ActiveForm.getBodyOverLay().style.visibility = "visible";

                            Test.Form._ActiveForm.getBase().style.borderColor = "#AAAAAA";
                        }
                    }
                    Test.Form._ActiveForm = value;
                    if (Test.Form._ActiveForm != null) {
                        if (Test.Form._ActiveForm.getBase() != null) {
                            Test.Form._ActiveForm.getBodyOverLay().style.visibility = "collapse";
                            Test.Form._ActiveForm.getBase().style.borderColor = "#FBFBFB";
                            Test.Form._ActiveForm.bringToFront();
                        }
                    }
                }

            },
            createStartSearchInput: function () {
                var input = Bridge.merge(document.createElement('input'), {
                    type: "text"
                } );
                var InputFocused = false;

                input.id = "StartSearchInput";
                input.style.position = "absolute";
                input.style.width = "344px";
                input.style.height = "40px";
                input.style.left = "48px";
                input.style.top = "0";
                input.style.backgroundColor = "#3F3F3F";

                input.style.borderStyle = "none";
                input.style.padding = "0";
                input.style.margin = "0";

                input.style.fontFamily = "Segoe UI";
                input.style.fontSize = "11pt";
                input.style.textIndent = "13px";
                input.style.color = "black";

                input.placeholder = "Search the web and Windows";

                //	Heading.Style.FontFamily = "Segoe UI";		
                input.addEventListener("keyup", function (ev) {
                    var kev = ev;

                    if (kev.keyCode === 13) {
                        // create a new Form

                        var frm = new Test.FormBrowser();
                        frm.setLeft("100px");
                        frm.setTop("100px");
                        //https://www.bing.com/search?q=
                        //https://www.google.com/#q=
                        frm.navigate(System.String.format("https://www.bing.com/search?q={0}", input.value));
                        frm.show();

                        input.blur();
                    }
                });


                input.onmouseup = $_.Test.Form.f1;
                $(input).on("focus", function () {
                    input.style.backgroundColor = "#F3F3F3";
                    input.style.outline = "0";
                });
                $(input).on("focusout", function () {
                    input.style.backgroundColor = "#3F3F3F";
                    input.value = "";
                    InputFocused = false;
                });
                $(input).on("focusin", function () {
                    input.style.backgroundColor = "#F3F3F3";
                    InputFocused = true;
                });

                input.onmousedown = $_.Test.Form.f2;

                input.onmouseenter = function (ev) {
                    if (Test.Form.movingForm != null) {
                        return;
                    }
                    if (InputFocused) {
                        input.style.backgroundColor = "#F3F3F3";
                    } else {
                        input.style.backgroundColor = "#575757";
                    }
                };

                input.onmouseleave = function (ev) {
                    if (Test.Form.movingForm != null) {
                        return;
                    }

                    if (InputFocused) {
                        input.style.backgroundColor = "#F3F3F3";
                    } else {
                        input.style.backgroundColor = "#3F3F3F";
                    }
                };

                return input;
            },
            createStartButton: function () {
                var butt = document.createElement('div');

                butt.style.width = "48px";
                butt.style.height = "40px";
                butt.style.position = "absolute";
                butt.style.fontSize = "12pt";
                butt.style.background = Test.Form.WinIcon;

                butt.onmouseup = function (ev) {
                    if (Test.Form.movingForm != null) {
                        return;
                    }

                    ev.stopPropagation();
                    ev.preventDefault();

                    butt.style.background = Test.Form.WinIcon;
                };

                butt.onmousedown = function (ev) {
                    if (Test.Form.movingForm != null) {
                        return;
                    }

                    Test.Form.mouse_Down = true;

                    ev.stopPropagation();
                    ev.preventDefault();

                    butt.style.background = Test.Form.WinIconDown;

                    Test.Form.setActiveForm(null);
                };

                butt.onmouseenter = function (ev) {
                    if (Test.Form.movingForm != null) {
                        return;
                    }

                    if (Test.Form.mouse_Down) {
                        butt.style.background = Test.Form.WinIconDown;
                    } else {
                        butt.style.background = Test.Form.WinIconHover;
                    }
                };

                butt.onmouseleave = function (ev) {
                    if (Test.Form.movingForm != null) {
                        return;
                    }

                    butt.style.background = Test.Form.WinIcon;
                };

                return butt;
            },
            setup: function (parent) {
                if (parent === void 0) { parent = null; }
                var keyCodes = new (System.Collections.Generic.List$1(System.Int32))([61, 107, 173, 109, 187, 189]);

                document.addEventListener("keydown", function (ev) {
                    var kev = ev;

                    if (kev.ctrlKey && (keyCodes.contains(kev.which))) {
                        ev.preventDefault();
                    }
                });

                $(window).bind('mousewheel DOMMouseScroll', function (event) { if (event.ctrlKey == true) { event.preventDefault(); } });

                if (parent == null) {
                    Test.Form.parent = document.body;
                } else {
                    Test.Form.parent = parent;
                }

                Test.Form.setWindowHolder(document.createElement('div'));
                Test.Form.getWindowHolder().style.position = "absolute";
                //TaskBar.Style.VerticalAlign = VerticalAlign.Bottom;
                Test.Form.getWindowHolder().style.width = "100%";
                Test.Form.getWindowHolder().style.height = "-webkit-calc(100% - 40px)";
                Test.Form.getWindowHolder().style.top = "0";
                Test.Form.getWindowHolder().style.left = "0";
                Test.Form.getWindowHolder().style.backgroundColor = "cornflowerblue";
                Test.Form.getWindowHolder().style.zIndex = "0";
                Test.Form.getWindowHolder().style.overflow = "auto";
                //user-select: none;

                $(Test.Form.getWindowHolder()).css("user-select", "none");

                Test.Form.setTaskBar(document.createElement('div'));
                Test.Form.getTaskBar().style.position = "absolute";
                //TaskBar.Style.VerticalAlign = VerticalAlign.Bottom;
                Test.Form.getTaskBar().style.width = "100%";
                Test.Form.getTaskBar().style.height = "40px";
                Test.Form.getTaskBar().style.top = "-webkit-calc(100% - 40px)";
                Test.Form.getTaskBar().style.left = "0";
                Test.Form.getTaskBar().style.zIndex = (2147483647).toString();

                $(Test.Form.getTaskBar()).css("user-select", "none");

                Test.Form.getTaskBar().style.backgroundColor = "#101010";

                Test.Form.setButtonStart(Test.Form.createStartButton());

                Test.Form.setInputStartSearch(Test.Form.createStartSearchInput());

                var mouseMove = $_.Test.Form.f3;

                window.addEventListener("mouseup", $_.Test.Form.f4);

                window.addEventListener("mousemove", mouseMove);

                Test.Form.parent.appendChild(Test.Form.getWindowHolder());
                Test.Form.parent.appendChild(Test.Form.getTaskBar());

                Test.Form.getTaskBar().appendChild(Test.Form.getButtonStart());
                Test.Form.getTaskBar().appendChild(Test.Form.getInputStartSearch());
            },
            calculateZOrder: function () {
                if (Test.Form.visibleForm == null) {
                    return;
                }
                for (var i = 0; i < Test.Form.visibleForm.getCount(); i = (i + 1) | 0) {
                    if (Test.Form.visibleForm.getItem(i) != null && Test.Form.visibleForm.getItem(i).getBase() != null) {
                        $(Test.Form.visibleForm.getItem(i).getBase()).css("zIndex", ((i + 1) | 0));
                    }
                }
            }
        },
        prev_px: 0,
        prev_py: 0,
        config: {
            properties: {
                Base: null,
                Heading: null,
                Body: null,
                BodyOverLay: null,
                ButtonClose: null,
                ButtonExpand: null,
                ButtonMinimize: null,
                Owner: null,
                MinWidth: 200,
                MinHeight: 50,
                HeadingTitle: null
            }
        },
        ctor: function () {
            this.$initialize();
            this.setBase(document.createElement('div'));

            this.getBase().id = "Base";

            this.setHeading(document.createElement('div'));

            this.setHeadingTitle(document.createElement('span'));

            this.setBody(document.createElement('div'));
            this.setBodyOverLay(document.createElement('div'));

            this.getBodyOverLay().style.visibility = "collapse";

            this.getBase().appendChild(this.getHeading());
            this.getBase().appendChild(this.getBody());
            this.getBase().appendChild(this.getBodyOverLay());

            this.getBase().addEventListener("mousedown", Bridge.fn.bind(this, $_.Test.Form.f5));

            this.getBase().addEventListener("mousemove", Bridge.fn.bind(this, $_.Test.Form.f6));

            this.getBase().style.position = "absolute";

            this.getHeading().id = "Heading";
            this.getHeading().style.height = "29px";
            this.getHeading().style.width = "100%";
            this.getHeading().style.verticalAlign = "top";
            this.getHeading().style.cursor = "default";
            this.getHeading().style.backgroundColor = "white"; // "#007ACC";
            this.getHeading().style.marginTop = "0";
            this.getHeading().style.marginLeft = "0";
            this.getHeading().style.marginRight = "0";
            this.getHeading().style.marginBottom = "0";
            this.getHeading().style.paddingBottom = "1px";

            this.getHeadingTitle().style.textIndent = "3px";


            this.getHeading().addEventListener("mousedown", Bridge.fn.bind(this, $_.Test.Form.f7));

            this.getHeading().style.fontFamily = "Segoe UI";
            this.getHeading().style.textAlign = 7;

            this.setButtonClose(this.createFormButton(Test.Form.FormButtonType.Close));
            this.setButtonExpand(this.createFormButton(Test.Form.FormButtonType.Maximize));
            this.setButtonMinimize(this.createFormButton(Test.Form.FormButtonType.Minimize));

            this.getBody().addEventListener("mousedown", Bridge.fn.bind(this, $_.Test.Form.f8));

            $(this.getHeading()).css("user-select", "none").css("user-drag:", "none");

            $(this.getBase()).css("user-select", "none").css("user-drag:", "none");

            $(this.getHeadingTitle()).css("user-select", "none").css("user-drag:", "none");

            //user-select: none;

            this.getBody().id = "Body";
            this.getBody().style.top = "30px";
            this.getBody().style.height = "-webkit-calc(100% - 30px)"; // -webkit-calc(100% - 60px)
            this.getBody().style.width = "100%";
            this.getBody().style.position = "absolute";

            this.setHeight("480px");
            this.setWidth("640px");

            this.getBody().style.backgroundColor = "#F0F0F0";

            this.getBodyOverLay().style.top = "30px";
            this.getBodyOverLay().style.height = "-webkit-calc(100% - 30px)"; // -webkit-calc(100% - 60px)
            this.getBodyOverLay().style.width = "100%";
            this.getBodyOverLay().style.position = "absolute";
            this.getBodyOverLay().style.zIndex = (2147483647).toString();
            this.getBodyOverLay().style.opacity = "0";

            this.getBodyOverLay().addEventListener("mousedown", Bridge.fn.bind(this, $_.Test.Form.f9));

            this.getBase().style.borderStyle = "solid";
            this.getBase().style.borderWidth = "2px";
            this.getBase().style.borderColor = "#FBFBFB";
            $(this.getBase()).css("box-shadow", "0px 0px 63px -17px rgba(0,0,0,0.75)");

            this.getHeading().appendChild(this.getHeadingTitle());
            this.getHeading().appendChild(this.getButtonClose());
            this.getHeading().appendChild(this.getButtonExpand());
            this.getHeading().appendChild(this.getButtonMinimize());

            this.initialise();

        },
        getHeight: function () {
            return this.getBase().style.height;
        },
        setHeight: function (value) {
            this.getBase().style.height = value;
        },
        getWidth: function () {
            return this.getBase().style.width;
        },
        setWidth: function (value) {
            this.getBase().style.width = value;
        },
        getLeft: function () {
            return this.getBase().style.left;
        },
        setLeft: function (value) {
            this.getBase().style.left = value;
        },
        getTop: function () {
            return this.getBase().style.top;
        },
        setTop: function (value) {
            this.getBase().style.top = value;
        },
        getText: function () {
            return this.getHeadingTitle().innerHTML;
        },
        setText: function (value) {
            this.getHeadingTitle().innerHTML = value;
        },
        getBackColor: function () {
            return this.getBody().style.backgroundColor;
        },
        setBackColor: function (value) {
            this.getBody().style.backgroundColor = value;
        },
        getForeColor: function () {
            return this.getBody().style.color;
        },
        setForeColor: function (value) {
            this.getBody().style.color = value;
        },
        isVisible: function () {
            return this.getBase() != null && this.getBase().style.visibility === "visible";
        },
        initialise: function () {

        },
        onShowing: function () {

        },
        onShowed: function () {

        },
        onClosing: function () {

        },
        createFormButton: function (Type) {
            var butt = document.createElement('div');

            butt.style.width = "45px";
            butt.style.height = "29px";
            butt.style.position = "absolute";
            butt.style.fontSize = "12pt";

            switch (Type) {
                case Test.Form.FormButtonType.Close: 
                    butt.style.backgroundColor = "white";
                    butt.style.color = "black";
                    butt.style.left = "-webkit-calc(100% - 45px)";
                    butt.id = "Close";
                    butt.innerHTML = "&#10006";
                    butt.onmousedown = Bridge.fn.bind(this, function (ev) {
                        if (Test.Form.movingForm != null) {
                            return;
                        }
                        Test.Form.mouse_Down = true;

                        ev.stopPropagation();
                        ev.preventDefault();

                        butt.style.backgroundColor = "#F1707A";
                        butt.style.color = "white";

                        Test.Form.setActiveForm(this);
                    });
                    butt.onmouseup = Bridge.fn.bind(this, $_.Test.Form.f10);
                    butt.onmouseenter = Bridge.fn.bind(this, function (ev) {
                        if (Test.Form.movingForm != null) {
                            return;
                        }

                        this.setCursor("default");

                        if (Test.Form.mouse_Down) {
                            butt.style.backgroundColor = "#F1707A";
                        } else {
                            butt.style.backgroundColor = "#E81123";
                        }
                        butt.style.color = "white";
                    });
                    butt.onmouseleave = function (ev) {
                        if (Test.Form.movingForm != null) {
                            return;
                        }

                        butt.style.backgroundColor = "white";
                        butt.style.color = "black";
                    };
                    break;
                case Test.Form.FormButtonType.Maximize: 
                    butt.style.backgroundColor = "white";
                    butt.style.left = "-webkit-calc(100% - 91px)";
                    butt.style.color = "black";
                    butt.id = "Maximize";
                    butt.innerHTML = "&#9633;";
                    butt.style.fontSize = "14pt";
                    butt.onmouseup = function (ev) {
                        if (Test.Form.movingForm != null) {
                            return;
                        }

                        ev.stopPropagation();
                        ev.preventDefault();

                        Test.Form.mouse_Down = false;

                        butt.style.backgroundColor = "white";
                        butt.style.color = "black";
                    };
                    break;
                case Test.Form.FormButtonType.Minimize: 
                    butt.style.backgroundColor = "white";
                    butt.style.left = "-webkit-calc(100% - 137px)";
                    butt.style.color = "black";
                    butt.id = "Minimize";
                    butt.innerHTML = "&#8213;";
                    butt.onmouseup = function (ev) {
                        if (Test.Form.movingForm != null) {
                            return;
                        }

                        ev.stopPropagation();
                        ev.preventDefault();

                        Test.Form.mouse_Down = false;

                        butt.style.backgroundColor = "white";
                        butt.style.color = "black";
                    };
                    break;
                case Test.Form.FormButtonType.Restore: 
                    break;
                case Test.Form.FormButtonType.Help: 
                    break;
                default: 
                    butt.onmouseup = $_.Test.Form.f11;
                    break;
            }

            butt.onmousemove = $_.Test.Form.f12;

            if (Type !== Test.Form.FormButtonType.Close) {
                butt.onmousedown = Bridge.fn.bind(this, function (ev) {
                    if (Test.Form.movingForm != null) {
                        return;
                    }

                    Test.Form.mouse_Down = true;

                    ev.stopPropagation();
                    ev.preventDefault();

                    butt.style.backgroundColor = "#CACACA";
                    butt.style.color = "black";

                    Test.Form.setActiveForm(this);
                });

                butt.onmouseenter = Bridge.fn.bind(this, function (ev) {
                    if (Test.Form.movingForm != null) {
                        return;
                    }
                    this.setCursor("default");

                    if (Test.Form.mouse_Down) {
                        butt.style.backgroundColor = "#CACACA";
                    } else {
                        butt.style.backgroundColor = "#E5E5E5";
                    }
                    butt.style.color = "black";
                });

                butt.onmouseleave = function (ev) {
                    if (Test.Form.movingForm != null) {
                        return;
                    }

                    butt.style.backgroundColor = "white";
                    butt.style.color = "black";
                };
            }

            butt.style.top = "0";

            butt.style.padding = "0";
            butt.style.margin = "0";
            butt.style.borderWidth = "0";

            butt.style.fontFamily = "Lucida Sans Unicode";
            butt.style.textAlign = "center";

            return butt;

        },
        setCursor: function (cur) {
            this.getBase().style.cursor = cur;
            this.getHeading().style.cursor = cur;
            //ButtonClose.Style.Cursor = cur;
            //ButtonExpand.Style.Cursor = cur;
            //ButtonMinimize.Style.Cursor = cur;
        },
        changeSelectionState$1: function (TurnOff) {
            if (TurnOff === void 0) { TurnOff = true; }
            this.changeSelectionState(Test.Form.parent.children, TurnOff);
        },
        changeSelectionState: function (Children, TurnOff) {
            var $t;
            if (TurnOff === void 0) { TurnOff = true; }
            if (Children == null) {
                return;
            }

            $t = Bridge.getEnumerator(Children);
            while ($t.moveNext()) {
                var item = $t.getCurrent();
                if (Bridge.referenceEquals(item.tagName.toLowerCase(), "input") || Bridge.referenceEquals(item.tagName.toLowerCase(), "span") || Bridge.referenceEquals(item.tagName.toLowerCase(), "textarea")) {
                    if (TurnOff) {
                        $(item).css("user-select", "none");
                    } else {
                        $(item).css("user-select", "text");
                    }
                }
                if (item.childElementCount > 0) {
                    this.changeSelectionState(item.children, TurnOff);
                }
            }
        },
        show: function (owner) {
            if (owner === void 0) { owner = null; }
            if (!Test.Form.visibleForm.contains(this)) {
                this.onShowing();

                if (owner == null) {
                    Test.Form.getWindowHolder().appendChild(this.getBase());
                    owner = Test.Form.getWindowHolder();
                } else {
                    owner.appendChild(this.getBase());
                }

                this.setOwner(owner);

                this.getBase().style.visibility = "visible";

                this.getBody().focus();

                Test.Form.visibleForm.add(this);

                Test.Form.calculateZOrder();

                this.onShowed();
            }
            Test.Form.setActiveForm(this);
        },
        bringToFront: function () {
            if (Test.Form.visibleForm.getCount() > 1 && !Bridge.referenceEquals(System.Linq.Enumerable.from(Test.Form.visibleForm).lastOrDefault(null, null), this)) {
                Test.Form.visibleForm.remove(this);
                Test.Form.visibleForm.add(this);
            }

            Test.Form.calculateZOrder();
        },
        close: function () {
            this.onClosing();

            Test.Form.setActiveForm(Test.Form._PrevActiveForm);

            Test.Form.visibleForm.remove(this);

            if (this.getBase() != null) {
                $(this.getBase()).empty();
                this.getBase().remove();
                this.setBase(null);
            }

            Test.Form.calculateZOrder();
        }
    });

    Bridge.ns("Test.Form", $_);

    Bridge.apply($_.Test.Form, {
        f1: function (ev) {
            if (Test.Form.movingForm != null) {
                return;
            }

            ev.stopPropagation();
        },
        f2: function (ev) {
            if (Test.Form.movingForm != null) {
                return;
            }

            Test.Form.mouse_Down = true;

            ev.stopPropagation();

            Test.Form.setActiveForm(null);
        },
        f3: function (ev) {
            if (Test.Form.movingForm != null) {
                if (Test.Form.movingForm.getBodyOverLay().style.visibility === "collapse") {
                    Test.Form.movingForm.getBodyOverLay().style.visibility = "visible";
                    Test.Form.movingForm.changeSelectionState$1(true);
                    Test.Form.movingForm.getHeading().focus();
                }

                var mev = ev;

                var obj = $(Test.Form.movingForm.getBase());

                var Y = (((mev.clientY + Test.Form.movingForm.prev_py) | 0)); // MovingForm.prev_py  Global.ParseInt(obj.Css("top")) + 
                var X = (((mev.clientX + Test.Form.movingForm.prev_px) | 0)); // - MovingForm.prev_px // Global.ParseInt(obj.Css("left")) + 

                var X1;
                var Y1;

                var W;
                var H;

                if (Y < 0) {
                    Y = 1;
                }

                ev.stopPropagation();

                switch (Test.Form.moveAction) {
                    case Test.Form.MouseMoveAction.Move: 
                        obj.css("top", Y);
                        obj.css("left", X);
                        break;
                    case Test.Form.MouseMoveAction.TopLeftResize: 
                        X1 = parseInt(obj.css("left"));
                        Y1 = parseInt(obj.css("top"));
                        W = parseInt(obj.css("width"));
                        H = parseInt(obj.css("height"));
                        W = (W - (((X - X1) | 0))) | 0;
                        H = (H - (((Y - Y1) | 0))) | 0;
                        if (W < Test.Form.movingForm.getMinWidth()) {
                            X = (X - (((Test.Form.movingForm.getMinWidth() - W) | 0))) | 0;
                            W = Test.Form.movingForm.getMinWidth();
                        }
                        if (H < Test.Form.movingForm.getMinHeight()) {
                            Y = (Y - (((Test.Form.movingForm.getMinHeight() - H) | 0))) | 0;
                            H = Test.Form.movingForm.getMinHeight();
                        }
                        obj.css("top", Y);
                        obj.css("left", X);
                        obj.css("width", W);
                        obj.css("height", H);
                        break;
                    case Test.Form.MouseMoveAction.TopResize: 
                        Y1 = parseInt(obj.css("top"));
                        H = parseInt(obj.css("height"));
                        H = (H - (((Y - Y1) | 0))) | 0;
                        if (H < Test.Form.movingForm.getMinHeight()) {
                            Y = (Y - (((Test.Form.movingForm.getMinHeight() - H) | 0))) | 0;
                            H = Test.Form.movingForm.getMinHeight();
                        }
                        obj.css("top", Y);
                        obj.css("height", H);
                        break;
                    case Test.Form.MouseMoveAction.TopRightResize: 
                        Y1 = parseInt(obj.css("top"));
                        X1 = parseInt(obj.css("left"));
                        W = parseInt(obj.css("width"));
                        H = parseInt(obj.css("height"));
                        H = (H - (((Y - Y1) | 0))) | 0;
                        W = (mev.clientX - X1) | 0;
                        if (H < Test.Form.movingForm.getMinHeight()) {
                            Y = (Y - (((Test.Form.movingForm.getMinHeight() - H) | 0))) | 0;
                            H = Test.Form.movingForm.getMinHeight();
                        }
                        if (W < Test.Form.movingForm.getMinWidth()) {
                            W = Test.Form.movingForm.getMinWidth();
                        }
                        obj.css("top", Y);
                        obj.css("height", H);
                        obj.css("width", W);
                        break;
                    case Test.Form.MouseMoveAction.LeftResize: 
                        X1 = parseInt(obj.css("left"));
                        W = parseInt(obj.css("width"));
                        W = (W - (((X - X1) | 0))) | 0;
                        if (W < Test.Form.movingForm.getMinWidth()) {
                            X = (X - (((Test.Form.movingForm.getMinWidth() - W) | 0))) | 0;
                            W = Test.Form.movingForm.getMinWidth();
                        }
                        obj.css("left", X);
                        obj.css("width", W);
                        break;
                    case Test.Form.MouseMoveAction.BottomLeftResize: 
                        X1 = parseInt(obj.css("left"));
                        Y1 = parseInt(obj.css("top"));
                        W = parseInt(obj.css("width"));
                        H = parseInt(obj.css("height"));
                        W = (W - (((X - X1) | 0))) | 0;
                        H = (mev.clientY - Y1) | 0;
                        if (W < Test.Form.movingForm.getMinWidth()) {
                            X = (X - (((Test.Form.movingForm.getMinWidth() - W) | 0))) | 0;
                            W = Test.Form.movingForm.getMinWidth();
                        }
                        if (H < Test.Form.movingForm.getMinHeight()) {
                            H = Test.Form.movingForm.getMinHeight();
                        }
                        obj.css("left", X);
                        obj.css("width", W);
                        obj.css("height", H);
                        break;
                    case Test.Form.MouseMoveAction.BottomResize: 
                        Y1 = parseInt(obj.css("top"));
                        H = parseInt(obj.css("height"));
                        H = (mev.clientY - Y1) | 0;
                        if (H < Test.Form.movingForm.getMinHeight()) {
                            H = Test.Form.movingForm.getMinHeight();
                        }
                        obj.css("height", H);
                        break;
                    case Test.Form.MouseMoveAction.RightResize: 
                        X1 = parseInt(obj.css("left"));
                        W = parseInt(obj.css("width"));
                        W = (mev.clientX - X1) | 0;
                        if (W < Test.Form.movingForm.getMinWidth()) {
                            W = Test.Form.movingForm.getMinWidth();
                        }
                        obj.css("width", W);
                        break;
                    case Test.Form.MouseMoveAction.BottomRightResize: 
                        Y1 = parseInt(obj.css("top"));
                        H = parseInt(obj.css("height"));
                        X1 = parseInt(obj.css("left"));
                        W = parseInt(obj.css("width"));
                        W = (mev.clientX - X1) | 0;
                        H = (mev.clientY - Y1) | 0;
                        if (H < Test.Form.movingForm.getMinHeight()) {
                            H = Test.Form.movingForm.getMinHeight();
                        }
                        if (W < Test.Form.movingForm.getMinWidth()) {
                            W = Test.Form.movingForm.getMinWidth();
                        }
                        obj.css("width", W);
                        obj.css("height", H);
                        break;
                    default: 
                        break;
                }
            }
        },
        f4: function (ev) {
            if (Test.Form.movingForm != null) {
                Test.Form.movingForm.getBodyOverLay().style.visibility = "collapse";
                Test.Form.movingForm.changeSelectionState$1(false);
            }

            Test.Form.movingForm = null;
            Test.Form.mouse_Down = false;
            Test.Form.moveAction = Test.Form.MouseMoveAction.Move;

        },
        f5: function (ev) {
            var mev = ev;
            Test.Form.mouse_Down = true;

            var obj = $(this.getBase());

            this.prev_px = (parseInt(obj.css("left")) - mev.clientX) | 0;
            this.prev_py = (parseInt(obj.css("top")) - mev.clientY) | 0;

            if (mev.layerX <= Test.Form.resizeCorners && mev.layerY <= Test.Form.resizeCorners) {
                this.setCursor("nwse-resize");

                Test.Form.moveAction = Test.Form.MouseMoveAction.TopLeftResize;
            } else if (mev.layerY <= Test.Form.resizeCorners && mev.layerX >= ((parseInt(this.getWidth()) - Test.Form.resizeCorners) | 0)) {
                this.setCursor("nesw-resize");

                Test.Form.moveAction = Test.Form.MouseMoveAction.TopRightResize;
            } else if (mev.layerY <= Test.Form.resizeCorners) {
                this.setCursor("n-resize");

                Test.Form.moveAction = Test.Form.MouseMoveAction.TopResize;
            } else if (mev.layerX <= Test.Form.resizeCorners && mev.layerY >= ((parseInt(this.getHeight()) - Test.Form.resizeCorners) | 0)) {
                this.setCursor("nesw-resize");

                Test.Form.moveAction = Test.Form.MouseMoveAction.BottomLeftResize;
            } else if (mev.layerY >= ((parseInt(this.getHeight()) - Test.Form.resizeCorners) | 0) && mev.layerX >= ((parseInt(this.getWidth()) - Test.Form.resizeCorners) | 0)) {
                this.setCursor("nwse-resize");

                Test.Form.moveAction = Test.Form.MouseMoveAction.BottomRightResize;
            } else if (mev.layerY >= ((parseInt(this.getHeight()) - Test.Form.resizeCorners) | 0)) {
                this.setCursor("s-resize");

                Test.Form.moveAction = Test.Form.MouseMoveAction.BottomResize;
            } else if (mev.layerX <= Test.Form.resizeCorners) {
                this.setCursor("w-resize");

                Test.Form.moveAction = Test.Form.MouseMoveAction.LeftResize;

            } else if (mev.layerX >= ((parseInt(this.getWidth()) - Test.Form.resizeCorners) | 0)) {
                this.setCursor("e-resize");

                Test.Form.moveAction = Test.Form.MouseMoveAction.RightResize;
            } else {
                this.setCursor("default");

                Test.Form.moveAction = Test.Form.MouseMoveAction.Move;
                return;
            }

            this.changeSelectionState$1();

            Test.Form.movingForm = this;
            mev.stopPropagation();
        },
        f6: function (ev) {
            var mev = ev;
            if (Test.Form.movingForm != null && Test.Form.moveAction === Test.Form.MouseMoveAction.Move) {
                this.getBase().style.cursor = "default";
                this.getHeading().style.cursor = "default";
                return;
            }

            if (Test.Form.moveAction === Test.Form.MouseMoveAction.TopLeftResize || mev.layerX <= Test.Form.resizeCorners && mev.layerY <= Test.Form.resizeCorners) {
                this.getBase().style.cursor = "nwse-resize";
                this.getHeading().style.cursor = "nwse-resize";
            } else if (Test.Form.moveAction === Test.Form.MouseMoveAction.TopRightResize || mev.layerY <= Test.Form.resizeCorners && mev.layerX >= ((parseInt(this.getWidth()) - Test.Form.resizeCorners) | 0)) {
                this.getBase().style.cursor = "nesw-resize";
                this.getHeading().style.cursor = "nesw-resize";
            } else if (mev.layerY <= Test.Form.resizeCorners || Test.Form.moveAction === Test.Form.MouseMoveAction.TopResize) {
                this.getBase().style.cursor = "n-resize";
                this.getHeading().style.cursor = "n-resize";
            } else if (Test.Form.moveAction === Test.Form.MouseMoveAction.BottomLeftResize || mev.layerX <= Test.Form.resizeCorners && mev.layerY >= ((parseInt(this.getHeight()) - Test.Form.resizeCorners) | 0)) {
                this.getBase().style.cursor = "nesw-resize";
                this.getHeading().style.cursor = "nesw-resize";
            } else if (Test.Form.moveAction === Test.Form.MouseMoveAction.BottomRightResize || mev.layerY >= ((parseInt(this.getHeight()) - Test.Form.resizeCorners) | 0) && mev.layerX >= ((parseInt(this.getWidth()) - Test.Form.resizeCorners) | 0)) {
                this.getBase().style.cursor = "nwse-resize";
                this.getHeading().style.cursor = "nwse-resize";
            } else if (Test.Form.moveAction === Test.Form.MouseMoveAction.BottomResize || mev.layerY >= ((parseInt(this.getHeight()) - Test.Form.resizeCorners) | 0)) {
                this.getBase().style.cursor = "s-resize";
                this.getHeading().style.cursor = "s-resize";
            } else if (Test.Form.moveAction === Test.Form.MouseMoveAction.LeftResize || mev.layerX <= Test.Form.resizeCorners) {
                this.getBase().style.cursor = "w-resize";
                this.getHeading().style.cursor = "w-resize";
            } else if (Test.Form.moveAction === Test.Form.MouseMoveAction.RightResize || mev.layerX >= ((parseInt(this.getWidth()) - Test.Form.resizeCorners) | 0)) {
                this.getBase().style.cursor = "e-resize";
                this.getHeading().style.cursor = "e-resize";
            } else {
                this.getBase().style.cursor = "default";
                this.getHeading().style.cursor = "default";
            }

        },
        f7: function (ev) {
            if (Test.Form.moveAction !== Test.Form.MouseMoveAction.Move) {
                Test.Form.setActiveForm(this);
                Test.Form.movingForm = null;
            }
            Test.Form.movingForm = this;

            Test.Form.setActiveForm(this);
        },
        f8: function (ev) {
            Test.Form.setActiveForm(this);
        },
        f9: function (ev) {
            this.getBodyOverLay().style.visibility = "collapse";
            Test.Form.setActiveForm(this);
        },
        f10: function (ev) {
            if (Test.Form.movingForm != null) {
                return;
            }

            ev.stopPropagation();
            ev.preventDefault();

            this.close();
        },
        f11: function (ev) {
            if (Test.Form.movingForm != null) {
                return;
            }

            ev.stopPropagation();
            ev.preventDefault();

            Test.Form.mouse_Down = false;
        },
        f12: function (ev) {
            if (Test.Form.movingForm != null) {
                return;
            }

            ev.stopImmediatePropagation();
            ev.preventDefault();
        }
    });

    Bridge.define("Test.Form.FormButtonType", {
        $kind: "enum",
        statics: {
            Close: 0,
            Maximize: 1,
            Minimize: 2,
            Restore: 3,
            Help: 4
        }
    });

    Bridge.define("Test.Form.MouseMoveAction", {
        $kind: "enum",
        statics: {
            Move: 0,
            TopLeftResize: 1,
            LeftResize: 2,
            BottomLeftResize: 3,
            BottomResize: 4,
            BottomRightResize: 5,
            RightResize: 6,
            TopResize: 7,
            TopRightResize: 8
        }
    });

    Bridge.define("Test.FormBrowser", {
        inherits: [Test.Form],
        config: {
            properties: {
                Content: null,
                URL: null
            }
        },
        navigate: function (url) {
            if (!System.String.startsWith(url.toLowerCase(), "http")) {
                url = System.String.concat("http://", url);
            }
            this.setURL(url);
            if (this.isVisible()) {
                this.getContent().src = this.getURL();
            }
        },
        initialise: function () {
            //Location = new HTMLInputElement();
            //Location.Style.Position = Position.Absolute;
            //Location.Style.Top = "1px";
            //Location.Style.Left = "1px";
            //Location.Style.Width = "-webkit-calc(100% - 6px)";
            //Location.Style.Height = "28px";

            //Location.Disabled = true;
            //Location.Style.Padding = "0";            

            //Content = new HTMLIFrameElement();
            //Content.Style.Position = Position.Absolute;
            //Content.Style.Top = "36px";
            //Content.Style.Left = "1px";
            //Content.Style.Width = "-webkit-calc(100% - 6px)";
            //Content.Style.Height = "-webkit-calc(100% - 44px)";

            this.setContent(document.createElement('iframe'));
            this.getContent().style.position = "absolute";
            this.getContent().style.top = "1px";
            this.getContent().style.left = "1px";
            this.getContent().style.width = "-webkit-calc(100% - 6px)";
            this.getContent().style.height = "-webkit-calc(100% - 6px)";

            this.setText("Quick Search");

            this.getContent().style.padding = "0";

            //Body.AppendChild(Location);
            this.getBody().appendChild(this.getContent());
        },
        onShowed: function () {
            if (!Bridge.referenceEquals(this.getURL(), this.getContent().src)) {
                this.getContent().src = this.getURL();
            }
        }
    });

    Bridge.define("Test.FormNotePad", {
        inherits: [Test.Form],
        config: {
            properties: {
                NotePadContent: null
            }
        },
        initialise: function () {
            this.setNotePadContent(document.createElement('textarea'));

            this.getNotePadContent().style.position = "absolute";
            this.getNotePadContent().style.width = "-webkit-calc(100% - 7px)";
            this.getNotePadContent().style.height = "-webkit-calc(100% - 7px)";

            this.getNotePadContent().style.top = "1px";
            this.getNotePadContent().style.left = "1px";
            this.getNotePadContent().style.resize = "none";

            this.getBody().appendChild(this.getNotePadContent());
        }
    });
});
