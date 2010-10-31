using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhea
{
    public static class SList
    {
        public static ISList<T> Cons<T>(T head, ISList<T> tail)
        {
            return new SListCons<T>(head, tail);
        }
        
        public static ISList<T> Nil<T>()
        {
            return SListNil<T>.Instance;
        }
        
        public static ISList<T> List<T>(params T[] objects)
        {
            return objects.Reverse().Aggregate(
                Nil<T>(), (acc, elem) => Cons<T>(elem, acc)
            );
        }
        
        public static ISList<T> ToSList<T>(this Stack<T> stack)
        {
            return stack.Aggregate(
                Nil<T>(), (acc, elem) => Cons<T>(elem, acc)
            );
        }
        
        public static TAccumulate Aggregate<TSource, TAccumulate>(
            this ISList<TSource> list,
            TAccumulate acc,
            Func<TAccumulate, TSource, TAccumulate> func
        )
        {
            while (!list.IsNil())
            {
                acc = func(acc, list.Head);
                list = list.Tail;
            }
            return acc;
        }
        
        public static TAccumulate AggregateRight<TSource, TAccumulate>(
            this ISList<TSource> list,
            TAccumulate acc,
            Func<TSource, TAccumulate, TAccumulate> func
        )
        {
            if (list.IsNil())
            {
                return acc;
            }
            return func(list.Head, AggregateRight(list.Tail, acc, func));
        }
        
        public static ISList<T> Reverse<T>(this ISList<T> list)
        {
            return Aggregate(list, Nil<T>(), (acc, elem) => Cons<T>(elem, acc));
        }
        
        public static ISList<T> Append<T>(this ISList<T> list1, ISList<T> list2)
        {
            return AggregateRight(list1, list2, Cons<T>);
        }
        
        public static string Show<T>(this ISList<T> list)
        {
            if (list.IsNil())
            {
                return "[]";
            }
            return string.Format(
                "[{0}]",
                list.Tail.Aggregate(
                    list.Head.ToString(),
                    (acc, elem) => string.Format("{0}, {1}", acc, elem)
                )
            );
        }
        
        public static bool ContainsList<T>(this ISList<T> list1, ISList<T> list2)
        {
            if (list2.IsNil() || list1 == list2)
            {
                return true;
            }
            else if (list1.IsNil())
            {
                return false;
            }
            return ContainsList(list1.Tail, list2);
        }
        
        public static ISList<T> GetPreviousList<T>(this ISList<T> list1, ISList<T> list2)
        {
            if (list1.Tail == list2)
            {
                return list1;
            }
            return GetPreviousList(list1.Tail, list2);
        }
    }
}
