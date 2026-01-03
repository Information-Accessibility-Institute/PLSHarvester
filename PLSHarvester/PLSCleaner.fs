namespace PLSHarvester

module PlsCleaner =

    /// Remove low-frequency items and optionally those already present in an external lexicon
    let clean
        (minFrequency: int)
        (existingWords: Set<string> option)
        (freqMap: Map<string,int>)
        : Map<string,int> =

        freqMap
        |> Map.filter (fun word count ->
            count >= minFrequency &&
            word.Length >= 2 &&   // Single Kanji characters are often noise
            match existingWords with
            | None -> true
            | Some dic -> not (dic.Contains word)
        )
