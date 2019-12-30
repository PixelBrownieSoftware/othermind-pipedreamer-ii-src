﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EVENT_TYPES
{
    LABEL = -1,
    MOVEMNET,
    DIALOGUE,
    SET_HEALTH = 2,
    RUN_CHARACTER_SCRIPT,
    ANIMATION = 4,
    SOUND,
    SET_FLAG = 7,
    CHECK_FLAG,
    CAMERA_MOVEMENT,
    BREAK_EVENT = 10,
    SET_UTILITY_FLAG = 12,
    FADE,
    CREATE_OBJECT,
    DISPLAY_CHARACTER_HEALTH,
    UTILITY_INITIALIZE,
    UTILITY_CHECK,
    WAIT,
    CHOICE,
    CHANGE_SCENE,
    PUT_SHUTTERS,
    DISPLAY_IMAGE,
    SHOW_TEXT,
    CHANGE_MAP,
    DEPOSSES,
    DELETE_OBJECT,
    SET_OBJ_COLLISION,
    ADD_CHOICE_OPTION,
    CLEAR_CHOICES,
    PRESENT_CHOICES
}
public enum LOGIC_TYPE
{
    VAR_GREATER,
    VAR_EQUAL,
    VAR_LESS,
    ITEM_OWNED,
    NUM_OF_GEMS,
    CHECK_UTILITY_RETURN_NUM,
    CHECK_CHARACTER,
    CHECK_CHARACTER_NOT,
    VAR_NOT_EQUAL
}
