using AutoMapper;

namespace LibraryService.MapProfile
{
    public static class AutoMapperExtensions
    {
        public static IMappingExpression<TSource, TDestination>
            IgnoreNullAndEmpty<TSource, TDestination>(
                this IMappingExpression<TSource, TDestination> expression)
        {
            expression.ForAllMembers(opt =>
                opt.Condition((src, dest, srcMember) =>
                {
                    if (srcMember is string str)
                        return !string.IsNullOrWhiteSpace(str);

                    return srcMember != null;
                }));

            return expression;
        }
    }
}
