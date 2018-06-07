#include "HelloWorldScene.h"
#include "SimpleAudioEngine.h"

USING_NS_CC;

Scene* HelloWorld::createScene()
{
    return HelloWorld::create();
}

// Print useful error message instead of segfaulting when files are not there.
static void problemLoading(const char* filename)
{
    printf("Error while loading: %s\n", filename);
    printf("Depending on how you compiled you might have to add 'Resources/' in front of filenames in HelloWorldScene.cpp\n");
}

// on "init" you need to initialize your instance
bool HelloWorld::init()
{
    //////////////////////////////
    // 1. super init first
    if ( !Scene::init() )
    {
        return false;
    }

    auto visibleSize = Director::getInstance()->getVisibleSize();
    Vec2 origin = Director::getInstance()->getVisibleOrigin();

    /////////////////////////////
    // 2. add a menu item with "X" image, which is clicked to quit the program
    //    you may modify it.

    // add a "close" icon to exit the progress. it's an autorelease object
    auto closeItem = MenuItemImage::create(
                                           "CloseNormal.png",
                                           "CloseSelected.png",
                                           CC_CALLBACK_1(HelloWorld::menuCloseCallback, this));

    if (closeItem == nullptr ||
        closeItem->getContentSize().width <= 0 ||
        closeItem->getContentSize().height <= 0)
    {
        problemLoading("'CloseNormal.png' and 'CloseSelected.png'");
    }
    else
    {
        float x = origin.x + visibleSize.width - closeItem->getContentSize().width/2;
        float y = origin.y + closeItem->getContentSize().height/2;
        closeItem->setPosition(Vec2(x,y));
    }


	auto label2 = Label::createWithTTF("LabelMenu", "fonts/Marker Felt.ttf", 24);
	auto menuItem_2 = MenuItemLabel::create(label2, CC_CALLBACK_1(HelloWorld::menu2Callback, this));
	menuItem_2->setPosition(Point(visibleSize.width / 2, visibleSize.height - 140));

	auto sprite1 = Sprite::create("timg.jpg");
	auto sprite2 = Sprite::create("timg1.jpg");
	auto item3 = MenuItemSprite::create(sprite1, sprite2);
	item3->setPosition(Vec2(Point(visibleSize.width / 2, visibleSize.height - 180)));

	MenuItemFont::setFontSize(30);
	//使用系统自带的Courier New字体
	MenuItemFont::setFontName("Courier New");
	//创建MenuItemFont对象
	auto *menuFont = MenuItemFont::create("FontMenu", CC_CALLBACK_1(HelloWorld::menu2Callback, this));
	menuFont->setTag(1);
	menuFont->setPosition(Point(visibleSize.width / 2, visibleSize.height - 100));
    // create menu, it's an autorelease object
    auto menu = Menu::create(closeItem,menuFont, menuItem_2,item3, NULL);
    menu->setPosition(Vec2::ZERO);
    this->addChild(menu, 1,100);

    /////////////////////////////
    // 3. add your codes below...



    // add a label shows "Hello World"
    // create and initialize a label

    auto label = Label::createWithTTF("16340282", "fonts/Marker Felt.ttf", 24);
	// 创建词典类实例，将xml文件加载到词典中
	auto *chnStrings = Dictionary::createWithContentsOfFile("label.xml");
	//通过xml文件中的key获取value
	const char *str1 = ((String*)chnStrings->objectForKey("name"))->getCString();
	auto label1 = Label::create(str1, "fonts/msyh.ttf", 24);
    if (label == nullptr||label1==nullptr)
    {
        problemLoading("'fonts/Marker Felt.ttf'");
    }
    else
    {
        // position the label on the center of the screen
        label->setPosition(Vec2(origin.x + visibleSize.width/2,
                                origin.y + visibleSize.height - label->getContentSize().height - label1->getContentSize().height));
		label1->setPosition(Vec2(origin.x + visibleSize.width / 2,
			origin.y + visibleSize.height-label1->getContentSize().height));

        // add the label as a child to this layer
		this->addChild(label1, 1);
        this->addChild(label, 1);
    }

    // add "HelloWorld" splash screen"
    auto sprite = Sprite::create("sysu.jpg");
    if (sprite == nullptr)
    {
        problemLoading("'HelloWorld.png'");
    }
    else
    {
        // position the sprite on the center of the screen
        sprite->setPosition(Vec2(visibleSize.width/2 + origin.x, visibleSize.height/2 + origin.y));

        // add the sprite as a child to this layer
        this->addChild(sprite, 0);
    }
    return true;
}


void HelloWorld::menuCloseCallback(Ref* pSender)
{
    //Close the cocos2d-x game scene and quit the application
    Director::getInstance()->end();

    #if (CC_TARGET_PLATFORM == CC_PLATFORM_IOS)
    exit(0);
#endif

    /*To navigate back to native iOS screen(if present) without quitting the application  ,do not use Director::getInstance()->end() and exit(0) as given above,instead trigger a custom event created in RootViewController.mm as below*/

    //EventCustom customEndEvent("game_scene_close_event");
    //_eventDispatcher->dispatchEvent(&customEndEvent);


}

void HelloWorld::menu2Callback(Ref* pSender) {
	//获取menuItemFont2
	CCMenuItemFont* menuItemFont2 = (CCMenuItemFont*)pSender;
	menuItemFont2->setString("You Click!");	
}
