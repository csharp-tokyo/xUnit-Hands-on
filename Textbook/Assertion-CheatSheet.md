# Assertionチートシート

|Assertion|否定形・非同期|概要|
|--|--|--|
|All||指定した複数の値がActionをすべてパスするか？|
|Collection||指定した複数の値が、指定されたすべてのActionをすべてパスするか？|
|Contains|DoesNotContain|コレクションに対象が含まれるか？|
|Empty|NotEmpty|コレクションが空か？|
|Single||コレクションに一つだけ要素が含まれるか？|
|Equal|NotEqual|同値か？（コレクションは全要素を比較）|
|StrictEqual|NotStrictEqual|厳密な同値検証（EqualsとGetHashCodeのovverideが必要）|
|Same|NotSame|同インスタンスか？|
|Raises|RaisesAsync|イベントが発行されたか？|
|RaisesAny|RaisesAnyAsync|Raisesと異なり、イベント引数が派生クラスでも可|
|Throws|ThrowsAsync|例外が発行あれたか？|
|ThrowsAny|ThrowsAnyAsync|Throwsと異なり、派生例外でも可|
|Null|NotNull|nullか？|
|PropertyChanged|PropertyChangedAsync|指定プロパティが変更通知されるか？|
|InRange|NotInRange|値が範囲内に収まるか？|
|Subset|Superset|ISetが別のISetのサブセットorスーパーセットかどうか？|
|ProperSubset|ProperSuperset|Subset/Supersetと異なり、同数・同要素を許容しない|
|StartsWith||文字列が前方一致するか？|
|EndsWith||文字列が後方一致するか？|
|Matches|DoesNotMatch|文字列がパターンにマッチするか？|
|IsType|IsNotType|オブジェクトが指定されたTypeか？|
|IsAssignableFrom||IsTypeとこなり派生Typeでも可|
