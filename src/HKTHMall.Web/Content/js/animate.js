// JavaScript Document
//图片轮播
//写完这个可以申请加薪么
//

;;(function(window, $, undefined) {
	var isAnimation = typeof history.pushState == "function";
	// 一些方法
	$.powerSwitch = function(elements, options) {
		$(elements).powerSwitch(options);
	};
	$.extend($.powerSwitch, {
		getRelative: function(trigger, params) {
			trigger = $(trigger);

			// 没有数据源,回家带孩子
			if (trigger.length == 0) return $();
			
			// 元素数组
			var arrTarget = [], isMoreToOne = false;
			trigger.each(function(index, element) {
                var selector = $(this).attr(params.attribute) || ($(this).attr("href") || "").split("#")[1];
				if (selector && arrTarget[selector] != true)  {
					var target = $();
					if (/^\w+$/.test(selector)) {
						target = $("#" + selector);
						// 如果属性值作为id没有对应元素,就作为类名选择器使用
						if (target.length === 0) {
							target = $("." + selector);
						}
						// 如果类名选择器也没有对应的元素,作为选择器使用
						if (target.length === 0) {
							target = $(selector);
						}
					} else {
						// 纯选择器
						target = $(selector);
					}

					target.each(function(index, element) {
						arrTarget.push(element);	
					});					
					
					// 设置标志量,避免重复
					arrTarget[selector] = true;
				} else if (arrTarget[selector] == true) {
					isMoreToOne = true;
				}
            });
			
			// 顺便判断下是否是多对一的关系
			trigger.data("isMoreToOne", isMoreToOne);

			return $(arrTarget);			
		},
		transition: function(target, duration, isReset) {
			var transform = "transform " + duration + "ms linear";
			if (isAnimation == false) return;
			// CSS3 transition设置
			if (isReset == true) {
				target.css("webkitTransition", "none").css("transition", "none")
					.data("hasTransition", false);
			} else if (!target.data("hasTransition")) {
				target.css({
					webkitTransition: "-webkit-" + transform,
					webkitBackfaceVisibility: "hidden",
					transition: transform,
					BackfaceVisibility: "hidden"
				}).data("hasTransition", true);
			}
		},
		translate: function(target, key, value) {
			// 偏移值设置
			var valueTransform = "translate"+ key +"("+ value +")";
			isAnimation? 
			target.css("webkitTransform", valueTransform).css("transform", valueTransform):
			target.css(key == "X"? { left: value }: { top: value });
		},
		animation: function(targetHide, targetShow, params) {
			var container = null, that = this, noAnimate = params.animation == "none";
			
			// 动画相关的几个小方法
			var funTransform = function(target, key, value) {
				// 如果value是纯数值
				if (parseInt(value) === value) value += "px";
				// IE10+等现代浏览器
				if (isAnimation) {
					// CSS3驱动动画					
					that.transition(target, params.duration, noAnimate);
					// 动画触发等
					that.translate(target, key, value);
					// console.log(value);
				} else {
					// IE6-IE9这些老弱病残
					// left/top
					target[noAnimate? "css": "animate"](key == "X"? {
						left: value
					}: {
						top: value	
					}, params.duration);
				}
			};
			
			// 以下方法旨在解决动画进行中仍然可以点击的问题
			if (params.duration && params.animation != "none") {
				params.isAnimating = true;
				// 为了简便,不走回调,直接定时器还原点击
				var durationObj = {
					"slow": 200,
					"normal": 400,
					"fast": 600	
				}, durationMs = durationObj[params.duration] || params.duration;
				
				if (params.direction == "sync") {
					if (targetHide && targetShow) {
						durationMs = 800;
					} else if (targetHide || targetShow) {
						durationMs = 400;
					} else {
						durationMs = 0;	
					}
				}

				setTimeout(function() {
					params.isAnimating = false;	
				}, durationMs);
			}			
			
			// 因为是万能切换,显然情况就比较复杂
			// 可以是列表元素动画,也可以是容器元素动画
			// 容器元素动画又分为两种,scroll和transform(IE6-9 left/top代替),自动判断
			// 列表元素动画也有几种,transform, fade, 和slide(toggle模式下专用)
			// 根据是否有target这个参数,决定是容器动画还是列表动画
			// 为了智能,容器动画根据一定的机制自动判断动画类型,这在Carousel模式下很有用
			// 智能判断的条件是:params.animation == "auto"
			// 动画的终点值与动画类型相关
			// 列表元素动画使用百分比,是定制无需关心
			// 容器元素动画的最终位置通过"data-position"存储访问
			// 写完这个叫老板加工资
			if ((targetShow && targetShow.length) || (targetHide && targetHide.length)) {
				// 列表动画
				// 一般用在选项卡,手风琴效果	
				// 有一些限制规则:
				// 1. 如果是多选模式,即一次可以有多个面板展开（手风琴效果）,不支持transform移动动画
				//    因此,此时,无动画显示
				if (params.toggle == true && params.animation == "translate") {
					params.animation = "none";
				}
				
				switch (params.animation) {
					case "translate": {
						// 移动
						// 比较前后移动元素的索引大小确定前后位置,
						// 用来决定移动的方向
						var indexHide = targetHide.data("index"),
							indexShow = targetShow.data("index");
							
						var objDirection = {
							"vertical": "Y",
							"horizontal": "X"	
						};
						
						if (indexHide != undefined && indexShow != undefined) {
							// 确定动画起点或终点位置是正的100%还是负的100%							
							var hundred = 100, isNext = true;
							// 共存在三种切换情况
							// 1. 定时
							// 2. 点击选项卡触发
							// 3. 点击前进后退按钮触发
							if (params.prevOrNext) {
								switch (params.prevOrNext.attr("data-type")) {
									case "prev": {
										isNext = false;
										break;
									}
									case "next": {
										isNext = true;
										break;
									}
									default: {
										// 这是点击选项卡
										// 根据前后的位置确定方向
										isNext = indexHide < indexShow;
									}
								}								
							}
							
							hundred = (isNext * 2 - 1 ) * 100;
							
							// 清除可能的transition
							// 因为动画的需要元素要改一下起始位置
							// 由于之前CSS3 transition的设置,这种位置变化会有动画效果,而我们需要的是瞬间移动（用户看不到的那种）
							that.transition(targetShow.show(), params.duration, true);
							// 要显示的元素乾坤大挪移到我们希望的位置
							that.translate(targetShow, objDirection[params.direction], hundred + "%");
							// 动画触发了,一个移走,一个移入
							setTimeout(function() {
								funTransform(targetHide, objDirection[params.direction], -1 * hundred + "%");
								funTransform(targetShow, objDirection[params.direction], "0%");	
							}, 17);
							
							// 清除触发源
							params.prevOrNext = null;					
						} else {
							// 索引缺失,直接显示隐藏
							targetHide.hide();
							targetShow.show();
						}
						
						break;
					}
					case "slide": {
						// 手风琴slideup/slidedown效果
						if (params.duration != "sync") {
							if (targetHide) targetHide.slideUp(params.duration);
							if (targetShow) targetShow.slideDown(params.duration);
						} else {
							if (targetHide) {
								targetHide.slideUp("normal", function() {
									if (targetShow) targetShow.slideDown();
								});
							} else if (targetShow) {
								targetShow.slideDown();
							}
						}						
						break;
					}
					case  "fade": {
						// 淡入淡出效果
						if (params.duration != "sync") {
							if (targetHide) targetHide.fadeOut(params.duration);
							if (targetShow) targetShow.fadeIn(params.duration);
						} else {
							if (targetHide) {
								targetHide.fadeOut("normal", function() {
									if (targetShow) targetShow.fadeIn();	
								});	
							} else if (targetShow) {
								targetShow.fadeIn();	
							}
						}
						break;
					}
					case  "visibility": {
						// visibility隐藏与显示
						if (targetHide) targetHide.css("visibility", "hidden");
						if (targetShow) targetShow.css("visibility", "visible");
						break;
					}
					default: {						
						// "auto", "none" 或其他乱七八糟类型直接display显隐
						if (targetHide) targetHide.hide();
						if (targetShow) targetShow.show();
					}
				}				
			} else if (params.container && params.container.length) {
				var position = params.container.data("position");
				container = params.container.get(0);
				
				// 容器动画
				// 各种模式都可能出现
				// 以下为各种动画类型的条件处理
				if (params.direction == "vertical") {
					// 根据容器是否存在滚动高度
					if (container.scrollHeight - container.clientHeight >= Math.max(position.top, 1)) {
						// scroll模式
						params.animation == "auto"? 
							params.container.animate({scrollTop: position.top}):
							params.container.scrollTop(position.top);
					} else {
						// transform模式
						funTransform(params.container, "Y", -1 * position.top)
					}
				} else {
					// 水平方向							
					if (container.scrollWidth - container.clientWidth >= Math.max(position.left, 1)) {
						// scroll模式
						params.animation == "auto"? 
							params.container.animate({"scrollLeft": position.left}):
							params.container.scrollLeft(position.left);
					} else {
						// transform模式
						funTransform(params.container, "X", -1 * position.left)
					}
				}
			}			
		}
	});
	
	$.fn.powerSwitch = function(options) {
		// 默认参数
		var defaults = {
			direction: "horizontal",
			eventType: "click",   // 其他可选参数:hover
			classAdd: "",         // 如果没有样式变化,请使用任意类名代替
			classRemove: "",
			classPrefix: "",      // eg. "prefix" → prefix_disabled, prefix_prev, prefix_play, prefix_pause, prefix_next
			attribute: "data-rel",
			animation: "auto",	  // 其他可选值:"none|display", "visibility", "translate", "fade", "slide"
			duration: 250,        // 动画持续时间,单位毫秒, 如果使用"sync"则表示同步
			container: null,
			autoTime: 0,          // 自动播放时间
			number: "auto",       // 每次切换的数目
			hoverDelay: 200,
			toggle: false,
			onSwitch: $.noop
		};
		// 最终参数
		var params = $.extend({}, defaults, options || {});
		
		// 动画是否正在进行
		params.isAnimating = false;
		
		// 一些全局类名		
		$.each(["disabled", "prev", "play", "pause", "next"], function(index, key) {
			key = $.trim(key);
			var upperKey = key.slice(0, 1).toUpperCase() + key.slice(1),
				paramsKey = "class" + upperKey,
				lastChar = params.classPrefix.slice(-1);
			if (params[paramsKey] === undefined) {
				if (params.classPrefix) {
					// 根据classPrefix中是否含关键字符（下划线或短横线）做判断
					if (/\-/g.test(params.classPrefix)) {
						params[paramsKey] = lastChar == "-"? 
							(params.classPrefix + key): [params.classPrefix, key].join("-");	
					} else if (/_/g.test(params.classPrefix)) {
						params[paramsKey] = lastChar == "_"? 
							(params.classPrefix + key): [params.classPrefix, key].join("_");	
					} else {
						// 驼峰-大小写组合
						params[paramsKey] = params.classPrefix + upperKey;
					}
				} else {
					params[paramsKey] = key;
				}
			}
		});
		
		
		// 一些全局变量 some global variables
		// 选中的触发项 the selected item
		var indexSelected = params.indexSelected || -1,
			numSwitch = parseInt(params.number) || 1,
		// hover延时处理的计时器 the timer for hover delay
		hoverTimer = null,
		// 自动播放的定时器
		autoPlayTimer = null,
		// 切换主体元素们
		eleRelatives = $(),
		// 主体元素的长度
		lenRelatives = 0;
		
		
		// 据说搞个变量速度更快~
		var self = $(this);
		
		// 无触发源,两种可能性
		// 1. 选择器挂掉了
		// 2. 单纯的自动播放,例如滚动新闻功能
		if (self.length == 0) {			
			// 如果是情况1,直接回家
			if (params.container == null || params.autoTime == 0) return self;
		}
		
		eleRelatives = $.powerSwitch.getRelative(self, params);

		if ((lenRelatives = eleRelatives.length) == 0) return self;
		
		// 确定indexSelected
		// 只有当未设定,或者不是toggle模式的时候
		if (indexSelected == -1 && params.toggle == false) {
			if (params.classAdd) {
				// 通过添加类名确定
				self.each(function(index, element) {
					if (indexSelected != -1) return;
                   	if ($(element).hasClass(params.classAdd)) indexSelected = index;
                });	
			} else {
				// 通过关联面板的显隐确定
				eleRelatives.each(function(index, element) {
					if (indexSelected != -1) return;
					if (params.animation == "visibility") {
						if ($(element).css("visibility") != "hidden") indexSelected = index;
					} else if ($(element).css("display") != "none") {
						indexSelected = index;
					}
				});	
			}
		}
		
		var isMoreToOne = false, elePrev = $(), eleNext = $(), elePrevOrNext = $();
		var funStatePrevNext = function(indexWill) {
			// 后退按钮的状态
			if (indexWill <= 0) {
				elePrev.addClass(params.classDisabled).removeAttr("title").attr("disabled", "disabled");
			} else {
				elePrev.removeClass(params.classDisabled).attr("title", elePrev.data("title")).removeAttr("disabled");
			}
			// 前进按钮的状态
			// 规则如下:
			// (总条目 - indexSelected位置值) / 每次切换的条数 是否大于 1
			if ((lenRelatives - indexWill) / numSwitch > 1) {
				eleNext.removeClass(params.classDisabled).attr("title", eleNext.data("title")).removeAttr("disabled");
			} else {
				eleNext.addClass(params.classDisabled).removeAttr("title").attr("disabled", "disabled");
			}
		}

		// 判断是否是多对一的关系
		if (self.eq(0).data("isMoreToOne") == true) {
			isMoreToOne = true;
			// 如果不是无限滚动
			if (params.classDisabled) {
				elePrev = self.eq(0), eleNext = self.eq(1);
				elePrev.data("title", elePrev.attr("title"));
				eleNext.data("title", eleNext.attr("title"));
				// 初始按钮的状态			
				funStatePrevNext(indexSelected);
				// 滚动位置
				if (indexSelected <= 0 && params.container) {
					$(params.container).scrollLeft(0).scrollTop(0);
				}
			} else if (params.container) {
				// 无限滚动
				// 克隆并载入				
				eleRelatives.clone().insertAfter(eleRelatives.eq(lenRelatives - 1));
				// 重新确定关联元素们
				eleRelatives = $.powerSwitch.getRelative(self, params);
				// more → one下之前点击的按钮
				// 用来确定自动播放(如果有)的方向
				// 默认是next方向
				elePrevOrNext = self.eq(1);
			} else {
				// 伪多对1,动画只能是fade或普通显隐
				elePrev = self.eq(0), eleNext = self.eq(1);	
				elePrevOrNext = eleNext;
			}
		}
		// 判断是否1对多
		var isOneToMore = false;
		if (self.length == 1 && lenRelatives > 1) {
			isOneToMore = true;
		}		
		
		// 切换的核心,所有的切换都要走这一步
		// 面向切换面板元素设计的切换方法
		var funSwitchable = function(indexWill) {			
			// 判断是否需要切换
			if (indexWill == indexSelected) {
				return;
			}
			// 总的切换项目数,每次切换的项目数
			var eleWillRelative = eleRelatives.slice(indexWill, indexWill + numSwitch);			
			var eleSelected = null, eleWillSelect = null, eleRelative = null;
			
			// 如果是toggle切换
			if (params.toggle == false) {
				// 在多对1模式下,我们关心的是触发按钮的临界状态	（disabled）等
				// 而不是选中与不选中的样式切换状态
				if (isMoreToOne == true) {
					// 偏移元素就是 eleWillRelative
					if (params.container) {					
						// 获取相对父元素的偏移
						var position = eleWillRelative.position();
						// 定位
						params.container = $(params.container);
						// 位置存储（动画终点位置）
						params.container.data("position", position);
						// 容器动画
						$.powerSwitch.animation(null, null, params);					
						// 按钮状态					
						params.classDisabled && funStatePrevNext(indexWill);
					} else {
						// 容器动画
						$.powerSwitch.animation(eleRelatives.eq(indexSelected, indexSelected + numSwitch), eleWillRelative, params);	
					}
					
					// 回调
					params.onSwitch.call(this, eleWillRelative);
				} else if (isOneToMore == true) {
					// 1对多模式
					// 也存在按钮的临界状态
					// 只能显示,不能收起
					// 对应元素的显隐控制
					$.powerSwitch.animation(null, eleWillRelative, params);
					// 回调
					params.onSwitch.call(this, eleWillRelative);
				} else if (indexSelected !== indexWill) {						
					// 1 vs 1
					// 关心按钮选中与不选中的样子
					eleWillSelect = self.eq(indexWill);
					if (indexSelected >= 0) {
						eleSelected = self.eq(indexSelected);
						eleRelative = eleRelatives.eq(indexSelected, indexSelected + numSwitch);
					} else {
						eleSelected = $();
						eleRelative = $();
					}

					// 触发元素的类名状态改变
					eleWillSelect.addClass(params.classAdd).removeClass(params.classRemove);
					// 已选元素的改变
					eleSelected.addClass(params.classRemove).removeClass(params.classAdd);
					// 对应元素的显隐控制
					$.powerSwitch.animation(eleRelative, eleWillRelative, params);
					// 回调
					params.onSwitch.call(this, eleWillRelative, eleSelected, eleRelative);
					
				}
				indexSelected = indexWill;
			} else {
				// 如果多选
				// 如果只能展开
				// 能伸能屈
				if ((params.animation == "visibility" && eleWillRelative.css("visibility") == "hidden") ||
				(params.animation != "visibility" && eleWillRelative.css("display") == "none")) {
					// 显示
					$.powerSwitch.animation(null, eleWillRelative, params);	
					display = true;
				} else {
					$.powerSwitch.animation(eleWillRelative, null, params);
					display = false;
				}			
				// 回调
				params.onSwitch.call(this, eleWillRelative, display);
			}
		};

		
		// 遍历 loop
		var anchorSplit = location.href.split("#")[1];
		
		self.each(function(index, element) {
			// 存储索引
			// 存储title以及index
			$(element).data("index", index);
			
			if (isMoreToOne == true) {		
				$(element).bind("click", function() {
					var indexWill, eleWill;				
					if (params.isAnimating == true) return false;
					if (params.classDisabled) {
						if ($(this).attr("disabled")) return false;
						if (index == 0) {
							indexWill = indexSelected - numSwitch;
							indexWill = Math.max(0, indexWill);
						} else if (index == 1) {
							indexWill = indexSelected + numSwitch;
							indexWill = Math.min(indexWill, lenRelatives - 1);
						}
						funSwitchable.call(this, indexWill);	
					} else if (params.container && lenRelatives > numSwitch) {
						// 无限滚动
						if (index == 0) {
							indexWill = indexSelected - numSwitch;
							if (indexWill < 0) {
								// 瞬间无感重定位
								eleWill = eleRelatives.eq(indexSelected + lenRelatives);
								$(params.container).data("position", eleWill.position());
								$.powerSwitch.animation(null, null, $.extend({}, params, { animation: "none" }));
								indexWill = indexSelected + lenRelatives - numSwitch;								
							}			
						} else if (index == 1) {
							indexWill = indexSelected + numSwitch;
							if (indexWill > lenRelatives * 2 - numSwitch) {
								// 末位数量不够了
								eleWill = eleRelatives.eq(indexSelected - lenRelatives);
								$(params.container).data("position", eleWill.position());
								$.powerSwitch.animation(null, null, $.extend({}, params, { animation: "none" }));
								// 新的索引位置
								indexWill = indexSelected - lenRelatives + numSwitch;
							}
						}
						funSwitchable.call(this, indexWill);	
						elePrevOrNext = $(this);			
					} else {
						index? funPlayNext(this): funPlayPrev(this);
						elePrevOrNext = $(this);
					}
					if (element && element.href) return false;
				});
			} else if (isOneToMore == true) {
				$(element).bind("click", function() {
					var indexWill;
					// 动画进行,则不能连续执行
					if (params.isAnimating == true) return false;
					
					if (params.number == "auto") {
						numSwitch = lenRelatives;
					}	
					if (!$(this).attr("disabled")) {
						if (indexSelected == -1) {
							indexWill = 0;
						} else {
							indexWill = indexSelected + numSwitch;					
						}
						
						funSwitchable.call(this, indexWill);
						if (indexWill >= lenRelatives - 1) {
							$(this).addClass(params.classDisabled).attr("disabled", "disabled").removeAttr("title");		
						}
					}
					if (element && element.href) return false;
				});
			} else if (params.eventType == "click") {				
				$(element).bind("click", function() {
					// 动画进行,则不能连续执行
					if (params.isAnimating == true) return false;
					// 设置标志量,根据位置判断方向
					params.prevOrNext = $(this);
					// 点击事件 click events
					funSwitchable.call(this, index);
					// 如果不是指向自身,或者带有href属性,阻止默认行为
					if (this.id !== $(this).attr(params.attribute) && (element && element.href)) {
						return false;
					}
				});
				
				if (anchorSplit && element.href && anchorSplit == element.href.split("#")[1]) {
					$(element).trigger("click");	
				}
			} else if (/^hover|mouseover$/.test(params.eventType)) {				
				$(element).hover(function() {
					if (params.isAnimating == true) return false;
					params.prevOrNext = $(this);
					// 鼠标经过 hover events
					clearTimeout(hoverTimer);
					hoverTimer = setTimeout(function() {
						funSwitchable.call(element, index);	
					}, parseInt(params.hoverDelay) || 0);
				}, function() {
					// 鼠标移开
					clearTimeout(hoverTimer);
				});
			}
        });
		
		eleRelatives.each(function(index, element) {
			$(element).data("index", index);	
		});
		
		// 自动播放
		var funPlayNext = function(trigger) {
			var indexWill = indexSelected + 1;
			if (indexWill >= lenRelatives) {
				indexWill = 0;
			}
			funSwitchable.call(trigger || self.get(indexWill), indexWill);
		}, funPlayPrev = function(trigger) {
			var indexWill = indexSelected - 1;
			if (indexWill < 0) {
				indexWill = lenRelatives -1;
			}
			funSwitchable.call(trigger || self.get(indexWill), indexWill);
		}, funPlayPrevOrNext = function() {
			elePrevOrNext.trigger("click");
		}, funAutoPlay = function() {
			clearTimeout(autoPlayTimer);
			if (funAutoPlay.flagAutoPlay == true) {
				autoPlayTimer = setTimeout(function() {
					isMoreToOne == false? funPlayNext(): funPlayPrevOrNext();					
					funAutoPlay();								
				}, params.autoTime);
			}
		};
		
		
		// 单对单模式,或者无限切换的多对一模式支持自动播放
		if ((isOneToMore == false && params.toggle == false && isMoreToOne == false) || (isMoreToOne == true && !params.classDisabled)) {			
			// 创建前进、后退、以及暂停按钮
			if (params.container && isMoreToOne == false) {
				var htmlTempOperate = '';
				self.length && $.each(["Prev", "Pause", "Next"], function(index, key) {
					if (params.autoTime == 0 && key == "Pause") return;
					// 自动播放模式时候需要
					htmlTempOperate = htmlTempOperate + '<a href="javascript:" class="'+ params["class" + key] +'" data-type="'+ key.toLowerCase() +'"></a>';	
				});
				
				params.container.append(htmlTempOperate).delegate("a", "click", function() {
					if (params.isAnimating == true) return false;
					var type = $(this).attr("data-type"), classType = params["class" + type.slice(0, 1).toUpperCase() + type.slice(1)],
						indexWill = indexSelected;
					switch (type) {
						case "prev": {
							params.prevOrNext = $(this);
							funPlayPrev();
							break	
						}
						case "play": {
							funAutoPlay.flagAutoPlay = true;
							$(this).attr("data-type", "pause").removeClass(classType).addClass(params.classPause);
							funAutoPlay();
							break	
						}
						case "pause": {
							funAutoPlay.flagAutoPlay = false;
							$(this).attr("data-type", "play").removeClass(classType).addClass(params.classPlay);
							funAutoPlay();
							break	
						}
						case "next": {
							params.prevOrNext = $(this);
							funPlayNext();
							break	
						}
					}
					
					return false;
				});			
			}
			
			if (params.autoTime) {
				// 定时播放相关事件绑定
				// 自定义按钮容器,选项卡,以及切换面板鼠标经过停止自动播放
				// 如果容器存在,且是包含关系
				// 只要绑定容器就可以
				if (params.hoverStop !== false) {
					var arrHoverPlay = [self, eleRelatives, params.container];
					if (isMoreToOne == true || (document.body.contains && params.container && params.container.get(0).contains(eleRelatives.get(0)))) {
						arrHoverPlay = [self, params.container];
					}
					$.each(arrHoverPlay, function(index, hoverTarget) {
						if (hoverTarget) hoverTarget.hover(function(event) {
							if (event.pageX !== undefined || params.eventType == "click") clearTimeout(autoPlayTimer);
						}, function(event) {
							if (event.pageX !== undefined || params.eventType == "click") funAutoPlay();
						});						
					});
				}
				
				funAutoPlay.flagAutoPlay = true;
				funAutoPlay();
			}
		}
		
		return self;
	};
})(window, jQuery);
;
// 个人中心判断左右高度
var heightleft = $("#PmainLeft").innerHeight();
var heightright = $("#PmainRight").innerHeight();
if (heightleft > heightright) {
	$("#PmainLeft").css({
		'borderRight':'1px solid #e2e1e1'
	});
	$("#PmainRight").css({
		'borderLeft':'0 none'
	});
}else if(heightleft <= heightright){
	$("#PmainLeft").css({
		'borderRight':'0 none'
	});
	$("#PmainRight").css({
		'borderLeft':'1px solid #e2e1e1'
	});
};
$("#caroEndless a").powerSwitch({
    classDisabled: '',
    container: $("#caroBoxEnd"),
    number: 2
});
$("#previous").powerSwitch({
    container: $("#boomCantainer")
});
//一级分类页面加载
$("#moreList").powerSwitch({ toggle: true });
//收货地址弹出框
for(var i = 0;i<$("#shouhuoAddress").find('a[name="change"]').length;i++){
$("#shouhuoAddress").find('a[name="change"]').eq(i).click(function(){
	$("#ShippingAddress").show();
	$("#mask").show().fadeIn();
	});
}
$("#addPlace").click(function () {
    ResetMessage();
    $("#txtUserAddressId").val("0");
	$("#ShippingAddress").show();
	$("#mask").show().fadeIn();
	});
$("#AddAddress").click(function(){
	$("#ShippingAddress").show().stop();
	$("#mask").show().stop();
	});
$("#AmendAddress").click(function(){
	$("#ShippingAddress").show().stop();
	$("#mask").show().stop();
	});	
$("#closeBut").click(function(){
	$("#ShippingAddress").hide().stop();
	$("#mask").hide().stop();
	});
$("#CountPay").click(function(){
	$("#LoginPay").show().stop();
	$("#mask").show().stop();
});
for(var i = 0;i<$("#shouhuoAddress").find('a[name="delete"]').length;i++){
$("#shouhuoAddress").find('a[name="delete"]').eq(i).click(function(){
	$("#delete").show();
	$("#mask").show().fadeIn();
	});
}
$('#tixianBut').click(function(){
	$('#tixian').show().stop();
	$('#mask').show().stop();
	});
$('#delcloseBut').click(function(){
	$('#tixian').hide().stop();
	$('#mask').hide().stop();
	});
$('#delcloseBut').click(function(){
	$('#delete').hide().stop();
	$('#mask').hide().stop();
});

//首页顶部的下来菜单
$("#Myhuika").hover(function(){
	$(this).css({
		'textAlign':'center',
		'borderTop':'1px solid #ed1c24',
		'borderLeft':'1px solid #ed1c24',
		'borderRight':'1px solid #ed1c24',
		'backgroundColor':'#ffffff'
	});
	$(this).find('i').hide().stop();
	$("#MyHK").slideDown(500).stop();
},function(){
	$(this).css({
		'borderTop':'1px solid #f8f8f9',
		'borderLeft':'1px solid #f8f8f9',
		'borderRight':'1px solid #f8f8f9',
		'background':'none'
	});
	$(this).find('i').show().stop();
	$("#MyHK").hide(500);
});
$("#help").hover(function(){
	$(this).css({
		'textAlign':'center',
		'borderTop':'1px solid #ed1c24',
		'borderLeft':'1px solid #ed1c24',
		'borderRight':'1px solid #ed1c24',
		'backgroundColor':'#ffffff'
	});
	$("#helpcenter").slideDown(500).stop();
	$(this).find('i').hide().stop();
},function(){
	$(this).css({
		'borderTop':'1px solid #f8f8f9',
		'borderLeft':'1px solid #f8f8f9',
		'borderRight':'1px solid #f8f8f9',
		'background':'none'
	});
	$("#helpcenter").hide(500);
	$(this).find('i').show().stop();
});
//商品详情页的选项卡
$(".tabNormal").powerSwitch({
    classAdd: "tab_on"
}).eq(0).trigger("click");
//判断有没有输入文字
$("#ShippingAddress .mamain-rimain .content .add-dz").find('input').eq(0).blur(function(){
	if($(this).val() == ''){
		$("#ShippingAddress .mamain-rimain .content .add-dz").find('.true').eq(0).hide();
		$("#ShippingAddress .mamain-rimain .content .add-dz").find('p[name="alert"]').eq(1).show();
	}else{
		$("#ShippingAddress .mamain-rimain .content .add-dz").find('.true').eq(0).show();
		$("#ShippingAddress .mamain-rimain .content .add-dz").find('p[name="alert"]').eq(1).hide();
	}
});
$("#ShippingAddress .mamain-rimain .content .add-dz").find('input[name="input4"]').blur(function(){
	if($(this).val() == ''){
		$("#ShippingAddress .mamain-rimain .content .add-dz").find('.true').eq(1).hide();
		$("#ShippingAddress .mamain-rimain .content .add-dz").find('p[name="alert"]').eq(2).show();
	}else{
		$("#ShippingAddress .mamain-rimain .content .add-dz").find('.true').eq(1).show();
		$("#ShippingAddress .mamain-rimain .content .add-dz").find('p[name="alert"]').eq(2).hide();
	}
});
//手机号
$("#ShippingAddress .mamain-rimain .content .add-dz").find('input[name="input5"]').blur(function(){
	if($(this).val() =="" || $(this).val().length !=11){
		$("#ShippingAddress .mamain-rimain .content .add-dz").find('.true').eq(2).hide();
		$("#ShippingAddress .mamain-rimain .content .add-dz").find('p[name="alert"]').eq(3).show();
	}else{
		$("#ShippingAddress .mamain-rimain .content .add-dz").find('.true').eq(2).show();
		$("#ShippingAddress .mamain-rimain .content .add-dz").find('p[name="alert"]').eq(3).hide();
	}
});
$("#ShippingAddress .mamain-rimain .content .add-dz").find('input[name="input2"]').blur(function(){
	if($(this).val() == ''){
		//$("#ShippingAddress .mamain-rimain .content .add-dz").find('.true').hide();
		$("#ShippingAddress .mamain-rimain .content .add-dz").find('p[name="alert"]').eq(4).show();
	}else{
		//$("#ShippingAddress .mamain-rimain .content .add-dz").find('.true').show();
		$("#ShippingAddress .mamain-rimain .content .add-dz").find('p[name="alert"]').eq(4).hide();
	}
});
$("#ShippingAddress .mamain-rimain .content .add-dz").find('input[name="input"]').blur(function(){
	if($(this).val() == ''){
		$("#ShippingAddress .mamain-rimain .content .add-dz").find('.true').eq(3).hide();
		$("#ShippingAddress .mamain-rimain .content .add-dz").find('p[name="alert"]').eq(5).show();
	}else{
		$("#ShippingAddress .mamain-rimain .content .add-dz").find('.true').eq(3).show();
		$("#ShippingAddress .mamain-rimain .content .add-dz").find('p[name="alert"]').eq(5).hide();
	}
});

//图片放大镜

//商品详情页面的判断
//tb-stock .tb-disable-increase, .tb-stock .tb-disable-increase:hover, .tb-stock .tb-disable-reduce, .tb-stock .tb-disable-reduce:hover
$('#countminus').click(function(){
	var minus = $('.DP_count .tb-stock').find('input[type="text"]').val();
	if(minus>1){
		minus --;
		$('.DP_count .tb-stock').find('input[type="text"]').val(minus);
	}else if(minus<=1){
		$('.DP_count .tb-stock').find('input[type="text"]').val(1);
	}
	
}).hover(function(){
	var minus = $('.DP_count .tb-stock').find('input[type="text"]').val();
	if(minus>1){
		$('#countminus').css({
			'cursor':'pointer'
		});
	}else if(minus<=1){
		$('#countminus').css({
			'cursor':'no-allowed'
		});
	}
});

$('#countplus').click(function(){
	var plus = $('.DP_count .tb-stock').find('input[type="text"]').val();
	plus ++;
	$('.DP_count .tb-stock').find('input[type="text"]').val(plus);
});


var zhongleicount = $(".yListr ul li").length;
;$(function(){
	$(".yListr ul li em").click(function(){
		$(this).addClass("yListrclickem").siblings().removeClass("yListrclickem");
	})
});
//alert($(".yListr ul li").eq(0).text());
$('.yListr .closered').find('a[name="closered"]').click(function(){
		$('.yListr').css({
			'marginBottom':'0',
			'border':'0 none',
			'paddingLeft':'0',
			'paddingBottom':'0',
			'backgroundColor':'none',
			'boxShadow':'0 0 0 '
		});
		$('.yListr .closered').hide();
		$('.yListr .gouwuBut').hide();
});
function emeach(){
	var num;
	$(".yListr ul").each(function(){
		num = $('li').find('em[class="yListrclickem"]').length;
	});

	return num;
}
$('#gouwuButSure').click(function(){
	var i = emeach();
	if(i == zhongleicount){
		$('.yListr').css({
			'marginBottom':'0',
			'border':'0 none',
			'paddingLeft':'0',
			'paddingBottom':'0',
			'backgroundColor':'none',
			'boxShadow':'0 0 0 '
		});
		$('.yListr .closered').hide();
		$('.yListr .gouwuBut').hide();
	}else{
		alert('请选择商品种类~');
	}
});
$('#gouwuche').click(function(){
	var i = emeach();
	if(i!=zhongleicount){
		$('.yListr').css({
			'marginBottom':'-64px',
			'border':'1px solid #ed1c24',
			'paddingLeft':'15px',
			'paddingBottom':'15px',
			'backgroundColor':'#fff',
			'boxShadow':'1px 1px 10px #ed1c24'
		});
		$('.yListr .closered').show();
		$('.yListr .gouwuBut').show();
	}else{
		$('#gouwuche').attr('href','###');
	}
});
$('#GoBuy').click(function(){
	var i = emeach();
	if(i!=zhongleicount){
		$('.yListr').css({
			'marginBottom':'-64px',
			'border':'1px solid #ed1c24',
			'paddingLeft':'15px',
			'paddingBottom':'15px',
			'backgroundColor':'#fff',
			'boxShadow':'1px 1px 10px #ed1c24'
		});
		$('.yListr .closered').show();
		$('.yListr .gouwuBut').show();
	}else{
		$('#gouwuche').attr('href','###');
	}
});

/*评价页面的星星*/
function neo(id){ //这里id
	var oStar = document.getElementById(id);
	var aLi = oStar.getElementsByTagName("li");
	var oUl = oStar.getElementsByTagName("ul")[0];
	var oSpan = oStar.getElementsByTagName("span")[1];
	var oP = oStar.getElementsByTagName("p")[0];
	var input=oStar.getElementsByTagName("input")[0];
	var i = iScore = iStar = 0;
	var aMsg = [
	]

	for (i = 1; i <= aLi.length; i++){
		aLi[i - 1].index = i;
		//鼠标移过显示分数
		aLi[i - 1].onmouseover = function (){
			fnPoint(this.index);
			//浮动层显示
			oP.style.display = "block";
			//计算浮动层位置
			oP.style.left = oUl.offsetLeft + this.index * this.offsetWidth - 104 + "px";
			//匹配浮动层文字内容
			//oP.innerHTML = "<em><b>" + this.index + "</b> 分 " + aMsg[this.index - 1].match(/(.+)\|/)[1] + "</em>" + aMsg[this.index - 1].match(/\|(.+)/)[1]
		};

		//鼠标离开后恢复上次评分
		aLi[i - 1].onmouseout = function (){
			//fnPoint(input.value);
			//关闭浮动层
			oP.style.display = "none"
		};
		//点击后进行评分处理
		aLi[i - 1].onclick = function (){
			iStar = this.index;
			oP.style.display = "none";
			//oSpan.innerHTML = "<strong>" + (this.index) + " 分</strong> (" + aMsg[this.index - 1].match(/\|(.+)/)[1] + ")";
			//input.value=this.index;
		}
	}

	//评分处理
	function fnPoint(iArg){
		//分数赋值
		iScore = iArg || iStar;
		for (i = 0; i < aLi.length; i++) aLi[i].className = i < iScore ? "on" : "";
	}

};

//图片放大镜2.0
(function($){

		$.fn.jqueryzoom = function(options){
		var settings = {
				xzoom: 200,//zoomed width default width
				yzoom: 200,//zoomed div default width
				offset: 10,	//zoomed div default offset
				position: "right",//zoomed div default position,offset position is to the right of the image
				lens:1, //zooming lens over the image,by default is 1;
				preload: 1
			};

			if(options) {
				$.extend(settings, options);
			}

		    var noalt='';
		    $(this).hover(function(){

		    var imageLeft = this.offsetLeft;
		    var imageRight = this.offsetRight;
		    var imageTop =  $(this).get(0).offsetTop;
		    var imageWidth = $(this).children('img').get(0).offsetWidth;
		    var imageHeight = $(this).children('img').get(0).offsetHeight;


            noalt= $(this).children("img").attr("alt");

		    var bigimage = $(this).children("img").attr("jqimg");

            $(this).children("img").attr("alt",'');

		    if($(".margPic .zoomdiv").get().length == 0){

		    $(this).after("<div class='zoomdiv' style='width:400px;height:400px;'><img class='bigimg' src='" + bigimage + "'/></div>");


		    $(this).append("<div class='jqZoomPup'>&nbsp;</div>");

		    }


		    if(settings.position == "right"){

            if(imageLeft + imageWidth + settings.offset + settings.xzoom > screen.width){

            leftpos = imageLeft  - settings.offset - settings.xzoom;

            }else{

		    leftpos = imageLeft + imageWidth + settings.offset;
            }
		    }else{
		    leftpos = imageLeft - settings.xzoom - settings.offset;
		    if(leftpos < 0){

            leftpos = imageLeft + imageWidth  + settings.offset;

		    }

		    }

		    $(".margPic .zoomdiv").css({ top: imageTop,left: leftpos });

		    $(".margPic .zoomdiv").width(settings.xzoom);

		    $(".margPic .zoomdiv").height(settings.yzoom);

            $(".margPic .zoomdiv").show();

            if(!settings.lens){
              $(this).css('cursor','crosshair');
			}
				   $(document.body).mousemove(function(e){
                   mouse = new MouseEvent(e);
                   /*$("div.jqZoomPup").hide();*/
				    var bigwidth = $(".bigimg").get(0).offsetWidth;
				    var bigheight = $(".bigimg").get(0).offsetHeight;
				    var scaley ='x';
				    var scalex= 'y';
				    if(isNaN(scalex)|isNaN(scaley)){
					    var scalex = (bigwidth/imageWidth);
					    var scaley = (bigheight/imageHeight);
					    $(".margPic .jqZoomPup").width((settings.xzoom)/scalex );
			    		$(".margPic .jqZoomPup").height((settings.yzoom)/scaley);
	                    if(settings.lens){
	                    $(".margPic .jqZoomPup").css('visibility','visible');
					}
					}
                    xpos = mouse.x - $(".margPic .jqZoomPup").width()/2 - imageLeft;
                    ypos = mouse.y - $(".margPic .jqZoomPup").height()/2 - imageTop ;
                    if(settings.lens){
                    xpos = (mouse.x - $(".margPic .jqZoomPup").width()/2 < imageLeft ) ? 0 : (mouse.x + $("div.jqZoomPup").width()/2 > imageWidth + imageLeft ) ?  (imageWidth -$("div.jqZoomPup").width() -2)  : xpos;
					ypos = (mouse.y - $(".margPic .jqZoomPup").height()/2 < imageTop ) ? 0 : (mouse.y + $("div.jqZoomPup").height()/2  > imageHeight + imageTop ) ?  (imageHeight - $("div.jqZoomPup").height() -2 ) : ypos;
                    }
                    if(settings.lens){
                    	$(".margPic .jqZoomPup").css({ top: ypos,left: xpos });
                    }
					scrolly = ypos;
					$(".margPic .zoomdiv").get(0).scrollTop = scrolly * scaley;
					scrollx = xpos;
					$(".margPic .zoomdiv").get(0).scrollLeft = (scrollx) * scalex ;
				    });
		    },function(){
               $(this).children("img").attr("alt",noalt);
		       $(document.body).unbind("mousemove");
		       if(settings.lens){
		       $(".margPic .jqZoomPup").remove();
		       }
		       $(".margPic .zoomdiv").remove();
		    });
        count = 0;
		if(settings.preload){
		$('body').append("<div style='display:none;' class='jqPreload"+count+"'>sdsdssdsd</div>");
		$(this).each(function(){
        var imagetopreload= $(this).children("img").attr("jqimg");
        var content = jQuery('.margPic .jqPreload'+count+'').html();
        jQuery('.margPic .jqPreload'+count+'').html(content+'<img src=\"'+imagetopreload+'\">');
		});
		}
		}

})(jQuery);

function MouseEvent(e) {
this.x = e.pageX
this.y = e.pageY
}
//=====================全局函数========================
//Tab控制函数
function tabs(tabId, tabNum){
	//设置点击后的切换样式
	$(tabId + " .tab li").removeClass("curr");
	$(tabId + " .tab li").eq(tabNum).addClass("curr");
	//根据参数决定显示内容
	$(tabId + " .tabcon").hide();
	$(tabId + " .tabcon").eq(tabNum).show();
}
//=====================全局函数========================

//==================图片详细页函数=====================
//鼠标经过预览图片函数
function preview(img){
	$('#items li img').addClass('addborder').siblings().removeClass('addborder');
	$("#preview .jqzoom img").attr("src",$(img).attr("src"));
	$("#preview .jqzoom img").attr("jqimg",$(img).attr("bimg"));
}

//图片放大镜效果
$(function(){
	$(".jqzoom").jqueryzoom({xzoom:380,yzoom:410});
});

//图片预览小图移动效果,页面加载时触发
$(function(){
	var tempLength = 0; //临时变量,当前移动的长度
	var viewNum = 4; //设置每次显示图片的个数量
	var moveNum = 2; //每次移动的数量
	var moveTime = 300; //移动速度,毫秒
	var scrollDiv = $(".spec-scroll .items ul"); //进行移动动画的容器
	var scrollItems = $(".spec-scroll .items ul li"); //移动容器里的集合
	var moveLength = scrollItems.eq(0).height() * moveNum; //计算每次移动的长度
	var countLength = (scrollItems.length - viewNum) * scrollItems.eq(0).height(); //计算总长度,总个数*单个长度
	  
	//下一张
	$(".spec-scroll .next").bind("click",function(){
		if(tempLength < countLength){
			if((countLength - tempLength) > moveLength){
				scrollDiv.animate({top:"-=" + moveLength + "px"}, moveTime);
				tempLength += moveLength;
			}else{
				scrollDiv.animate({top:"-=" + (countLength - tempLength) + "px"}, moveTime);
				tempLength += (countLength - tempLength);
			}
		}
	});
	//上一张
	$(".spec-scroll .prev").bind("click",function(){
		if(tempLength > 0){
			if(tempLength > moveLength){
				scrollDiv.animate({top: "+=" + moveLength + "px"}, moveTime);
				tempLength -= moveLength;
			}else{
				scrollDiv.animate({top: "+=" + tempLength + "px"}, moveTime);
				tempLength = 0;
			}
		}
	});
});

$('.shezhijiaoyi tr td').find('input[name="shezhi"]').eq(0).blur(function(){
	if($(this).val()== ''){
		$('.shezhijiaoyi tr td').find('p[name="wrong"]').eq(0).show();
		$('.shezhijiaoyi tr td').find('p[name="right"]').eq(0).hide();
		$(this).css({
			'border':'1px solid #ed1c24',
			'boxShadow':'1px 1px 10px #ed1c24'
		});
	}else{
		$('.shezhijiaoyi tr td').find('p[name="right"]').eq(0).show();
		$('.shezhijiaoyi tr td').find('p[name="wrong"]').eq(0).hide();
		$(this).css({
			'boxShadow':'0 0 0'
		});
	}
});
$('.shezhijiaoyi tr td').find('input[name="shezhi"]').eq(1).blur(function(){
	if($(this).val()== ''){
		$('.shezhijiaoyi tr td').find('p[name="wrong"]').eq(1).show();
		$('.shezhijiaoyi tr td').find('p[name="right"]').eq(1).hide();
		$(this).css({
			'border':'1px solid #ed1c24',
			'boxShadow':'1px 1px 10px #ed1c24'
		});
	}else{
		$('.shezhijiaoyi tr td').find('p[name="right"]').eq(1).show();
		$('.shezhijiaoyi tr td').find('p[name="wrong"]').eq(1).hide();
		$(this).css({
			'boxShadow':'0 0 0'
		});
	}
});
$('.shezhijiaoyi tr td').find('input[name="shezhi"]').eq(2).blur(function(){
	if($(this).val()== ''){
		$('.shezhijiaoyi tr td').find('p[name="wrong"]').eq(2).show();
		$('.shezhijiaoyi tr td').find('p[name="right"]').eq(2).hide();
		$(this).css({
			'border':'1px solid #ed1c24',
			'boxShadow':'1px 1px 10px #ed1c24'
		});
	}else{
		$('.shezhijiaoyi tr td').find('p[name="right"]').eq(2).show();
		$('.shezhijiaoyi tr td').find('p[name="wrong"]').eq(2).hide();
		$(this).css({
			'boxShadow':'0 0 0'
		});
	}
});































