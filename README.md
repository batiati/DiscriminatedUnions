## Discriminated Unions

A simple, no-overhead, value-type wrapper around `Object` to mimic [Algebraic Data Types](https://en.wikipedia.org/wiki/Algebraic_data_type) on C# (Something like [TypeScript's discriminated unions](https://www.typescriptlang.org/docs/handbook/unions-and-intersections.html#discriminating-unions), [Rust's enum](https://doc.rust-lang.org/book/ch06-01-defining-an-enum.html), [F#](https://docs.microsoft.com/en-us/dotnet/fsharp/language-reference/discriminated-unions), etc)

## Motivation

Algebraic data types are powerful concepts that help to design cleaner interfaces that are less susceptible to misuse.

For example:

```
//Some business object
public class User { ... }

//Some expected errors
public enum Error
{
  InvalidId,
  NotFound
}

static OneOf<User,Error> GetUser(int id)
{
    if (id == 0) return Error.InvalidId;
    var user = FetchFromDatabase(id);
    
    if (user == null) return Error.NotFound;
    return user;
}

static void Main()
{
  var ret = GetUser(42);
  
  if (ret.TryGet(out Error err))
  {
    Console.WriteLine("Error: {0}", err);
  }
  else if (ret.TryGet(out User user))
  {
    Console.WriteLine("Welcome {0}", user.Name);
  }
}
```

## Why not use some existing library (pick one)

There are many libraries out there that aim the same objective. I choose to write this one for those reasons:

1. **Light**, it is a ValueType, there is no more overhead than using `Object` and doing the type-checking by yourself.

2. **No intrusive**, its primary goal is just to guarantee the object construction using one of the expected types.
Methods TryGet and Equals/Cast overloading are optional to use, you can still access the raw `Value` and even use the new C# pattern matching features with it.

3. **Replaceable**, it is namespaced at `System` to avoid code pollution with strange usings, if someday C# introduces it's own discriminated type, a quick refactory can remove it altogether.

## Limitations

- Even overloading the implicit cast operators, there is no way to overload the `is` or `as` operators.

- TryGet will return `false` for both type if the value is `null`

- TryGet will return `true` for both if the same types or with inherited types were used, for example `OneOf<string,string>` or `OneOf<string,object>`


So, in some scenarios, it could be necessary to use the raw `Value` property directly, or even consider not to use `OneOf<>` at all.


For example, the same thing can be written this way:

```
void DeleteUser(OneOf<int, User> user)
{
    int id;
    if (user.TryGet(out int _id))
    {
        id = _id;
    }
    else if (user.TryGet(out User _user))
    {
        id = _user.ID;
    }
    else
    {
        throw new NullReferenceException();
    }

    DeleteFromDatabase(id);
}
```

Or this way:

```
void DeleteUser(OneOf<int, User> user)
{
    var id = user.Value switch
    {
        int value => value,
        User value => value.ID,
        _ => throw new NullReferenceException(),
    };

    DeleteFromDatabase(id);
}
```

Or whitout `OneOf<>`:

```
void DeleteUser(User user)
{
    if (user == null) throw new NullReferenceException();
    DeleteUser(user.ID);
}

void DeleteUser(int id)
{
    DeleteFromDatabase(id);
}
```

## TODO List

- Add serialization support

- Add support for 3 or more generic types

- Add nullable annotations

- Publish nuget package




