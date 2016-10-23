﻿using LanguageExt.Instances;
using LanguageExt.TypeClasses;
using static LanguageExt.TypeClass;
using static LanguageExt.Prelude;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LanguageExt.Instances
{
    /// <summary>
    /// Set type-class instance
    /// </summary>
    /// <typeparam name="A"></typeparam>
    public struct MSet<A> :
        MonadPlus<Set<A>, A>,
        Foldable<Set<A>, A>,
        Eq<Set<A>>,
        Monoid<Set<A>>
   {
        public Set<A> Append(Set<A> x, Set<A> y) =>
            Set.createRange(x.Concat(y));

        IEnumerable<B> BindSeq<MONADB, MB, B>(Set<A> ma, Func<A, MB> f)
            where MONADB : struct, Monad<MB, B>
        {
            foreach(var a in ma)
                foreach (var b in f(a).ToSeq<MONADB, MB, B>())
                    yield return b;
        }

        public MB Bind<MONADB, MB, B>(Set<A> ma, Func<A, MB> f) where MONADB : struct, Monad<MB, B> =>
            default(MONADB).Return(BindSeq<MONADB, MB, B>(ma, f));

        public int Count(Set<A> fa) =>
            fa.Count();

        public Set<A> Subtract(Set<A> x, Set<A> y) =>
            Set.createRange(Enumerable.Except(x, y));

        public Set<A> Empty() =>
            Set.empty<A>();

        public bool Equals(Set<A> x, Set<A> y) =>
            x == y;

        public Set<A> Fail(object err) =>
            Empty();

        public Set<A> Fail(Exception err = null) =>
            Empty();

        public S Fold<S>(Set<A> fa, S state, Func<S, A, S> f) =>
            fa.Fold(state, f);

        public S FoldBack<S>(Set<A> fa, S state, Func<S, A, S> f) =>
            fa.FoldBack(state, f);

        public Set<A> Plus(Set<A> ma, Set<A> mb) =>
            ma + mb;

        public Set<A> Return(IEnumerable<A> xs) =>
            Set.createRange(xs);

        public Set<A> Return(A x, params A[] xs) =>
            Set.createRange(x.Cons(xs));

        public Set<A> Zero() =>
            Empty();
    }
}