﻿using System.Collections.Generic;
using System.Linq;

namespace EasyNetQ.AmqpExceptions
{
    internal class AmqpException
    {
        public AmqpExceptionPreface Preface { get; }
        public IList<IAmqpExceptionElement> Elements { get; }

        public AmqpException(AmqpExceptionPreface preface, IList<IAmqpExceptionElement> elements)
        {
            Preface = preface;
            Elements = elements;
        }

        private int GetElement<T>() where T : AmqpExceptionIntegerValueElement
        {
            return Elements.OfType<T>().Select(x => x.Value).SingleOrDefault();
        }

        public int Code => GetElement<AmqpExceptionCodeElement>();
        public int ClassId => GetElement<AmqpExceptionClassIdElement>();
        public int MethodId => GetElement<AmqpExceptionMethodIdElement>();

        public const ushort ConnectionClosed = 320;
        public const ushort NotFound = 404;
    }

    public class AmqpExceptionPreface
    {
        public string Text { get; }

        public AmqpExceptionPreface(string text)
        {
            Text = text;
        }
    }

    public interface IAmqpExceptionElement { }

    public class TextElement : IAmqpExceptionElement
    {
        public string Text { get; }

        public TextElement(string text)
        {
            Text = text;
        }
    }

    public class AmqpExceptionKeyValueElement : IAmqpExceptionElement
    {
        public string Key { get; }
        public string Value { get; }

        public AmqpExceptionKeyValueElement(string key, string value)
        {
            Key = key;
            Value = value;
        }
    }

    public abstract class AmqpExceptionIntegerValueElement : IAmqpExceptionElement
    {
        public int Value { get; set; }
    }

    public class AmqpExceptionCodeElement : AmqpExceptionIntegerValueElement { }
    public class AmqpExceptionClassIdElement : AmqpExceptionIntegerValueElement { }
    public class AmqpExceptionMethodIdElement : AmqpExceptionIntegerValueElement { }
}
