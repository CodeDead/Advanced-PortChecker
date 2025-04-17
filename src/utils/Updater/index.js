const Updater = (os, architectureName, currentVersion) => {
  /**
   * Compare two semantic versions
   * @param ver1 Version 1
   * @param ver2 Version 2
   * @returns {number} 1 if ver1 > ver2, -1 if ver1 < ver2, 0 if equal
   */
  const semverCompare = (ver1, ver2) => {
    const v1Parts = ver1.slice(1).split('.').map(Number);
    const v2Parts = ver2.slice(1).split('.').map(Number);

    for (let i = 0; i < 3; i++) {
      if (v1Parts[i] > v2Parts[i]) return 1;
      if (v1Parts[i] < v2Parts[i]) return -1;
    }
    return 0;
  };

  /**
   * Parse the update data
   * @param data The update data
   * @returns {{updateUrl, infoUrl: *, version: SemVer, updateAvailable: boolean}} The parsed update data
   */
  const parseUpdate = (data) => {
    const platform = data.platforms.find(
      (p) => p.platformName.toLowerCase() === os.toLowerCase(),
    );
    if (!platform) {
      throw new Error(`Platform ${os} not found`);
    }

    // Find the architecture
    const architecture = platform.architectures.find(
      (a) => a.name === architectureName,
    );
    if (!architecture) {
      throw new Error(
        `Architecture ${architectureName} not found for platform ${os}`,
      );
    }

    // Sort releases by semver in descending order
    const sortedReleases = architecture.releases.sort((a, b) => {
      return semverCompare(b.semver, a.semver);
    });

    return {
      updateUrl: sortedReleases[0].downloadUrl,
      infoUrl: sortedReleases[0].infoUrl,
      version: sortedReleases[0].semver,
      updateAvailable:
        semverCompare(currentVersion, sortedReleases[0].semver) < 0,
    };
  };

  return new Promise((resolve, reject) => {
    fetch(
      'https://api.codedead.com/api/v1/applications/47cd7e8f-2744-443c-850e-619df5d5c43f',
    )
      .then((res) => {
        if (!res.ok) {
          throw Error(res.statusText);
        }
        return res.json();
      })
      .then((data) => resolve(parseUpdate(data)))
      .catch((error) => reject(error.toString()));
  });
};

export default Updater;
