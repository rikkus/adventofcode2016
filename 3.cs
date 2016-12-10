@"  785  516  744
  272  511  358
  [... etc ...]
"
.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries)
.Select(row => row.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries))
.Select(row => new[] { int.Parse(row[0]), int.Parse(row[1]), int.Parse(row[2]) })
.Count(row => (row.Sum() - row.Max()) > row.Max())
