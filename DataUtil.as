package com.finegamedesign.utils
{
    import flash.utils.getDefinitionByName;
    import flash.utils.getQualifiedClassName;

    public final class DataUtil
    {
         public static function CloneList(original:*):*
         {
             return original.concat();
         }

         public static function Length(data:*):int
         {
             return data.length;
         }

         public static function Clear(data:*):void
         {
             data.length = 0;
         }

         public static function Pop(arrayOrVector:*):void
         {
             arrayOrVector.pop();
         }

         public static function Shift(arrayOrVector:*):void
         {
             arrayOrVector.shift();
         }

         public static function Split(text:String, delimiter:String):Vector.<String>
         {
             var parts:Array = text.split(delimiter);
             var strings:Vector.<String> = new Vector.<String>();
             for (var p:int = 0; p < parts.length; p++) {
                 strings.push(parts[p]);
             }
             return strings;
         }

         public static function Join(texts:*, delimiter:String):String
         {
             return texts.join(delimiter);
         }

         /**
          * Only supports basic data types:  int, Number, Boolean, String, and vectors of those.
          */
         public static function ToList(... rest):*
         {
             var Type:Class = Class(getDefinitionByName(getQualifiedClassName(rest[0])));
             var aList:*;
             if (rest[0] is int) {
                 aList = new Vector.<int>();
             }
             else if (rest[0] is Number) {
                 aList = new Vector.<Number>();
             }
             else if (rest[0] is String) {
                 aList = new Vector.<String>();
             }
             else if (rest[0] is Boolean) {
                 aList = new Vector.<Boolean>();
             }
             else if (rest[0] is Vector.<int>) {
                 aList = new Vector.<Vector.<int>>();
             }
             else if (rest[0] is Vector.<Number>) {
                 aList = new Vector.<Vector.<Number>>();
             }
             else if (rest[0] is Vector.<String>) {
                 aList = new Vector.<Vector.<String>>();
             }
             else if (rest[0] is Vector.<Boolean>) {
                 aList = new Vector.<Vector.<Boolean>>();
             }
             else {
                 aList = new Array();
             }
             for (var i:int = 0; i < rest.length; i++) {
                 aList.push(rest[i]);
             }
             return aList;
         }
    }
}
