﻿using Scriban;
using Scriban.Syntax;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Xunit;

namespace ScribanExpress.UnitTests
{
    public class IfElseTests
    {
        [Fact]
        public void If()
        {
            var itemWrapper = new { item = new { someValue=true } };

            var templateText = @"{{ if item.someValue }}value is true{{ end }}";
            var template = Template.Parse(templateText, null, null, null);

            var result = AnonGenerate(itemWrapper, template.Page.Body);

            var functor = result.Compile();

            var sb = new StringBuilder();
            functor(sb, itemWrapper, null);

            sb.ToString().ShouldBe("value is true");
        }


        [Fact]
        public void IfElse()
        {
            var presonwrapper = new { item = new { someValue = false } };

            var templateText = @"{{ if item.someValue }}value is true{{ else }} value is not true {{ end }}";
            var template = Template.Parse(templateText, null, null, null);

            var result = AnonGenerate(presonwrapper, template.Page.Body);

            var functor = result.Compile();

            var sb = new StringBuilder();
            functor(sb, presonwrapper, null);
            sb.ToString().ShouldBe(" value is not true ");
        }


        [Fact]
        public void NotIf()
        {
            var presonwrapper = new { item = new { someValue = false } };

            var templateText = @"{{ if !item.someValue }}value is false{{ end }}";
            var template = Template.Parse(templateText, null, null, null);

            var result = AnonGenerate(presonwrapper, template.Page.Body);

            var functor = result.Compile();

            var sb = new StringBuilder();
            functor(sb, presonwrapper, null);
            sb.ToString().ShouldBe("value is false");
        }

        [Fact]
        public void ElseIf()
        {
            var itemWrapper = new { item = new { someValue = false, someOtherValue = true  } };

            var templateText = @"
{{ if item.someValue }}
  ...
{{ else if  item.someOtherValue }}
  some other value is true
{{ else }}
  all false
{{ end }}";
            var template = Template.Parse(templateText, null, null, null);

            var result = AnonGenerate(itemWrapper, template.Page.Body);
            var functor = result.Compile();

            // test else if
            var sb = new StringBuilder();
            functor(sb, itemWrapper, null);
            sb.ToString().ShouldContain("some other value is true");

            // test else else
            itemWrapper = new { item = new { someValue = false, someOtherValue = false } };
            sb = new StringBuilder();
            functor(sb, itemWrapper, null);
            sb.ToString().ShouldContain(" all false");
        }
        public Expression<Action<StringBuilder, T, object>> AnonGenerate<T>(T value, ScriptBlockStatement scriptBlockStatement)
        {
            return new ExpressionGenerator().Generate<T, object>(scriptBlockStatement);
        }
    }
}